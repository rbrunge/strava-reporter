using Nest;
using StravaReporter.Models.Strava;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StravaReporter.Repositories
{
    public interface ICacheRepository
    {
        Task<ActivitySummary> GetActivitySummaryAsync(long id);
        Task CreateOrUpdateActivitySummaryAsync(IEnumerable<ActivitySummary> activitySummary);
        Task<Activity> GetActivityAsync(long id);
        Task CreateOrUpdateActivityAsync(IEnumerable<Activity> activitySummary);
    }

    public class CacheRepository : ICacheRepository
    {
        private readonly IElasticClient _elasticClient;

        public CacheRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task CreateOrUpdateActivitySummaryAsync(IEnumerable<ActivitySummary> activitySummary)
        {
            var response = await _elasticClient.IndexManyAsync(activitySummary,  nameof(ActivitySummary).ToLowerInvariant());
        }

        public async Task<ActivitySummary> GetActivitySummaryAsync(long id)
        {
            var response = await _elasticClient.GetAsync(new DocumentPath<ActivitySummary>(id));
            return response.Source;
        }

        public async Task CreateOrUpdateActivityAsync(IEnumerable<Activity> activity)
        {
            var response = await _elasticClient.IndexManyAsync(activity, nameof(Activity).ToLowerInvariant());
        }

        public async Task<Activity> GetActivityAsync(long id)
        {
            var response = await _elasticClient.GetAsync<Activity>(id, idx => idx.Index(nameof(Activity).ToLowerInvariant()));
            return response.Source;
        }

    }
}
