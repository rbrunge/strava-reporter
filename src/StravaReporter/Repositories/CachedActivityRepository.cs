using System;
using System.Threading.Tasks;
using Nest;
using StravaReporter.Models.Strava;
using StravaReporter.Services;

namespace StravaReporter.Repositories
{
    public class CachedActivityRepository : DocumentActivityRepository
    {
        private static readonly object CacheLockObject = new object();
        private readonly DocumentActivityRepository _documentActivityRepository;

        public CachedActivityRepository(IElasticClient elasticClient, IAccessTokenProvider tokenProvider, DocumentActivityRepository documentActivityRepository) : base(elasticClient, tokenProvider)
        {
            _documentActivityRepository = documentActivityRepository;
        }

        public override Task<ActivitySummary> GetActivitySummaryAsync(long id)
        {
            var activity = ReadThrough<ActivitySummary>(
                cache: async () => await _documentActivityRepository.GetActivitySummaryAsync(id),
                remote: async () => await _documentActivityRepository.GetActivitySummaryAsync(id)
            );

            return Task.FromResult(activity);
        }

        private T ReadThrough<T>(Func<Task<T>> cache, Func<Task<T>> remote) where T : class
        {
            T t = (T)((object)cache().Result);
            if (t == null)
            {
                t = remote().Result;
            }
            return t;
        }

    }
}
