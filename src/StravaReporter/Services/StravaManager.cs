using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using StravaReporter.Models.Strava;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StravaReporter.Services
{
    public interface IAccessTokenProvider
    {
        string Token { get; }
    }

    public class HttpContextAccessTokenProvider : IAccessTokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ClaimsPrincipal _caller;
        public HttpContextAccessTokenProvider(IHttpContextAccessor contextAccessor, ClaimsPrincipal caller)
        {
            _contextAccessor = contextAccessor;
            _caller = caller;
        }
        public string Token => _caller.Claims.FirstOrDefault(c => c.Type == "urn:strava:accesstoken").Value;

        //public static void SetToken(Guid token)
        //{
        //    HttpContext.Current.Items["AccessToken"] = token;
        //}
    }
    public interface IStravaConnector
    {
        Task<string> GetDataAsync(string url);
    }

    public class StravaConnector : IStravaConnector
    {
        private readonly IAccessTokenProvider _tokenProvider;
        public StravaConnector(IAccessTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        private readonly string accessToken;
        public async Task<string> GetDataAsync(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = Constants.ApiBaseUrl;
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _tokenProvider.Token);
                var json = await client.GetStringAsync(url);
                return json;
            }
        }
    }

    public interface IStravaManager
    {
        Task<Activity> GetLatestAsync();
        Task<IEnumerable<Lap>> GetLapsAsync(int activityId);
    }

    public class StravaManager : IStravaManager
    {
        private readonly IStravaConnector _stravaConnector;
        public StravaManager(IStravaConnector stravaConnector)
        {
            _stravaConnector = stravaConnector;
        }

        public async Task<Activity> GetLatestAsync()
        {
            Activity activity = null;

            var latestJson = await _stravaConnector.GetDataAsync(Constants.ActivityLastestSummaryUrl);
            var latest = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<ActivitySummary>>(latestJson));
            var json = await _stravaConnector.GetDataAsync(string.Format(Constants.ActivityUrl, latest.FirstOrDefault().Id));
            activity = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Activity>(json));

            return activity;
        }
        public async Task<IEnumerable<Lap>> GetLapsAsync(int activityId)
        {
            IEnumerable<Lap> laps;

            var json = await _stravaConnector.GetDataAsync(string.Format(Constants.LapsUrl, activityId));
            laps = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<Lap>>(json));

            return laps;
        }
    }
}
