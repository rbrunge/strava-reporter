﻿#region MIT License

/*
	The MIT License (MIT)

	Copyright (c) 2013 Bombsquad Inc
    Copyright (c) 2016 ElasticIdentity

	Permission is hereby granted, free of charge, to any person obtaining a copy of
	this software and associated documentation files (the "Software"), to deal in
	the Software without restriction, including without limitation the rights to
	use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
	the Software, and to permit persons to whom the Software is furnished to do so,
	subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
	FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
	COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
	IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
	CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Elasticsearch.Net;
//using Microsoft.AspNet.Identity;
using Nest;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using Microsoft.AspNetCore.Mvc.Internal;

namespace ElasticIdentity
{
    public class ElasticUserStore<TUser> :
        ElasticUserStore,
        IUserStore<TUser>
        IUserLoginStore<TUser>,
        IUserClaimStore<TUser>,
        IUserRoleStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IUserTwoFactorStore<TUser, string>,
        IUserEmailStore<TUser, string>,
        IUserPhoneNumberStore<TUser, string>,
        IUserLockoutStore<TUser, string>
        where TUser : ElasticUser
    {
        public ElasticUserStore(IElasticClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            string index;
            client.ConnectionSettings.DefaultIndices.TryGetValue(typeof(TUser), out index);
            if (string.IsNullOrEmpty(index))
            {
                index = client.ConnectionSettings.DefaultIndex;
            }

            if (string.IsNullOrEmpty(index))
            {
                throw new ArgumentNullException("You must specify default index on IElasticClient either through MapDefaultTypeIndicies or DefaultIndex");
            }

            Client = client;
            Index = index;
        }

        public ElasticUserStore(Uri elasticServerUri, string indexName = "users")
            : this(new ElasticClient(new ConnectionSettings(elasticServerUri)
                .ThrowExceptions()
                .MapDefaultTypeIndices(x => x.Add(typeof(TUser), indexName))))
        {
        }

        public IElasticClient Client { get; private set; }

        public string Index { get; private set; }

        public virtual bool EnsureIndex(bool forceCreate = false)
        {
            var exists = Client.IndexExists(new IndexExistsRequest(Index)).Exists;

            if (exists && forceCreate)
            {
                Client.DeleteIndex(new DeleteIndexRequest(Index));
                exists = false;
            }

            if (!exists)
            {
                var response = Client.CreateIndex(Index, DescribeIndex);

                return AssertResponseSuccess(response);
            }

            return false;
        }

        public virtual ICreateIndexRequest DescribeIndex(CreateIndexDescriptor createIndexDescriptor)
        {
            return createIndexDescriptor.Mappings(m => m
                    .Map<TUser>(mm => mm
                        .AutoMap()
                        .AllField(af => af
                            .Enabled(false))));
        }

        private bool AssertResponseSuccess(IResponse response)
        {
            if (response.OriginalException != null)
            {
                throw response.OriginalException;
            }
            else
            {
                if (!response.ApiCall.Success)
                {
                    throw new Exception($"Error while creating index:\n{response.DebugInformation}");
                }
            }

            return true;
        }

        public virtual void Dispose()
        {
        }


        public virtual Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            //ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.Id.ToString());
        }

        public virtual Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.UserName);
        }

        public virtual Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.UserName = userName;
            return TaskCache.CompletedTask;
        }

        public virtual Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.NormalizedEmail);
        }

        public virtual Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.NormalizedEmail = normalizedEmail;
            return TaskCache.CompletedTask;
        }

        public async Task CreateAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(user.Id))
            {
                await Client.IndexAsync(user, x => x
                   .Refresh(Refresh.True));
            }
            else
            {
                await Client.IndexAsync(user, x => x
                   .OpType(OpType.Create)
                   .Refresh(Refresh.True));
            }
        }

        public async Task UpdateAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(user.Id))
            {
                throw new ArgumentNullException("user.Id", "A null or empty User.Id value is not allowed in UpdateAsync");
            }

            await Client.IndexAsync(user, x => x
                .Version(user.Version)
                .Refresh(Refresh.True));
        }

        public async Task DeleteAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            await Client.DeleteAsync(DocumentPath<TUser>.Id(user.Id), d => d
                .Version(user.Version)
                .Refresh(Refresh.True));
        }

        public async Task<TUser> FindByIdAsync(string userId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            var result = await Client.GetAsync(DocumentPath<TUser>.Id(userId));

            if (!result.IsValid || !result.Found)
                return null;

            var user = result.Source;
            user.Id = result.Id;
            user.Version = result.Version;

            return user;
        }

        public async Task<TUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));

            var result = await Client.SearchAsync<TUser>(s => s
                    .Version(true)
                    .Query(q => q
                        .Bool(b => b
                            .Filter(f => f
                                .Term(t => t
                                    .Field(tf => tf.UserName)
                                    .Value(userName.ToLowerInvariant()))))));

            return ProcessSearchResponse(result);
        }

        public async Task<TUser> FindByEmailAsync(string email)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));

            var result = await Client.SearchAsync<TUser>(s => s
                    .Version(true)
                    .Query(q => q
                        .Bool(b => b
                            .Filter(f => f
                                .Term(t => t
                                    .Field(tf => tf.Email.Address)
                                    .Value(email.ToLowerInvariant()))))));

            return ProcessSearchResponse(result);
        }

        public async Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            var result = await Client.SearchAsync<TUser>(s => s
                    .Query(q => q
                        .Bool(b => b
                        .Filter(f =>
                            f.Term(t1 => t1
                                .Field(tf1 => tf1.Logins.First().LoginProvider)
                                .Value(login.LoginProvider))
                            &&
                            f.Term(t2 => t2
                                .Field(tf2 => tf2.Logins.First().ProviderKey)
                                .Value(login.ProviderKey))))));

            return ProcessSearchResponse(result);
        }

        private TUser ProcessSearchResponse(ISearchResponse<TUser> result)
        {
            if (!result.IsValid || result.TerminatedEarly || result.TimedOut) return null;

            var user = result.Documents.FirstOrDefault();
            var hit = result.Hits.FirstOrDefault();

            if (user == null || hit == null) return null;

            user.Id = hit.Id;
            user.Version = hit.Version.GetValueOrDefault();

            return user;
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            //ThrowIfDisposed();

            if (user == null) throw new ArgumentNullException(nameof(user));
            if (login == null) throw new ArgumentNullException(nameof(login));

            user.Logins.Add(new ElasticUserLoginInfo
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            });
            return DoneTask;
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (login == null) throw new ArgumentNullException(nameof(login));

            user.Logins.RemoveAll(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);
            return DoneTask;
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult<IList<UserLoginInfo>>(user
                .Logins
                .Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, user.EmailAddress))
                .ToList());
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var result = (IList<Claim>)user
                .Claims
                .Select(x => x.AsClaim())
                .ToList();
            return Task.FromResult(result);
        }

        public Task AddClaimAsync(TUser user, Claim claim)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (claim == null) throw new ArgumentNullException(nameof(claim));

            user.Claims.Add(claim);
            return DoneTask;
        }

        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (claim == null) throw new ArgumentNullException(nameof(claim));

            user.Claims.Remove(claim);
            return DoneTask;
        }

        public Task AddToRoleAsync(TUser user, string role)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (role == null) throw new ArgumentNullException(nameof(role));

            user.Roles.Add(role);
            return DoneTask;
        }

        public Task RemoveFromRoleAsync(TUser user, string role)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (role == null) throw new ArgumentNullException(nameof(role));

            user.Roles.Remove(role);
            return DoneTask;
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var result = user.Roles.ToList();
            return Task.FromResult((IList<string>)result);
        }

        public Task<bool> IsInRoleAsync(TUser user, string role)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult(user.Roles.Contains(role));
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.PasswordHash = passwordHash;
            return DoneTask;
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.SecurityStamp = stamp;
            return DoneTask;
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.SecurityStamp);
        }

        public async Task<IEnumerable<TUser>> GetAllAsync()
        {
            // ToDo: Use scroll -> https://goo.gl/E86ezB
            // Due to the nature Elasticsearch allocates memory on the JVM heap for use in storing the results
            // you should not set the size to a very large value. Relying on the default size of 10 for now.
            var result = await Client.SearchAsync<TUser>(search => search.MatchAll());
            return result.Documents;
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.TwoFactorAuthenticationEnabled = enabled;
            return DoneTask;
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.TwoFactorAuthenticationEnabled);
        }

        public Task SetEmailAsync(TUser user, string email)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.Email = email == null
                ? null
                : new ElasticUserEmail { Address = email };
            return DoneTask;
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var elasticUserEmail = user.Email;

            return elasticUserEmail != null
                ? Task.FromResult(elasticUserEmail.Address)
                : Task.FromResult<string>(null);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var elasticUserEmail = user.Email;

            return elasticUserEmail != null
                ? Task.FromResult(elasticUserEmail.IsConfirmed)
                : Task.FromResult(false);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var elasticUserEmail = user.Email;
            if (elasticUserEmail != null)
                elasticUserEmail.IsConfirmed = true;
            else throw new InvalidOperationException("User have no configured email address");
            return DoneTask;
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.Phone = phoneNumber == null
                ? null
                : new ElasticUserPhone { Number = phoneNumber };
            return DoneTask;
        }

        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var elasticUserPhone = user.Phone;

            return elasticUserPhone != null
                ? Task.FromResult(elasticUserPhone.Number)
                : Task.FromResult<string>(null);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var elasticUserPhone = user.Phone;

            return elasticUserPhone != null
                ? Task.FromResult(elasticUserPhone.IsConfirmed)
                : Task.FromResult(false);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.Phone == null) throw new ArgumentNullException("TUser.Phone");

            user.Phone.IsConfirmed = true;
            return DoneTask;
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.LockoutEndDate);
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.LockoutEndDate = lockoutEnd;
            return DoneTask;
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.AccessFailedCount++);
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.AccessFailedCount = 0;
            return DoneTask;
        }

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Enabled);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.Enabled = enabled;
            return DoneTask;
        }


        private void ThrowIfDisposed()
        {
        }

    }

    public abstract class ElasticUserStore
    {
        protected static readonly Task DoneTask = Task.FromResult(true);
        protected const int DefaultSizeForAll = 1000 * 1000;
    }
}