using System;
using Nest;
using StravaReporter.Models.Strava;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StravaReporter.Services;

namespace StravaReporter.Repositories
{
    public class DocumentActivityRepository : IActivityRepository
    {
        private readonly IElasticClient _elasticClient;
        private readonly IAccessTokenProvider _tokenProvider;

        public DocumentActivityRepository(IElasticClient elasticClient, IAccessTokenProvider tokenProvider)
        {
            _elasticClient = elasticClient;
            _tokenProvider = tokenProvider;
        }

        public async Task CreateOrUpdateActivitySummaryAsync(IEnumerable<ActivitySummary> activitySummary)
        {
            if (_elasticClient == null) throw new ArgumentNullException(nameof(_elasticClient));
            if (activitySummary == null) throw new ArgumentNullException(nameof(activitySummary));
            var indexManyAsync = _elasticClient.IndexManyAsync(activitySummary, nameof(ActivitySummary).ToLowerInvariant());
            if (indexManyAsync != null)
            {
                var response = await indexManyAsync;
            }
        }

        public virtual async Task<ActivitySummary> GetActivitySummaryAsync(long id)
        {
            if (_elasticClient == null) throw new ArgumentNullException(nameof(_elasticClient));
            var @async = _elasticClient.GetAsync(new DocumentPath<ActivitySummary>(id));
            if (@async != null)
            {
                var response = await @async;
                if (response != null) return response.Source;
            }
            return null;
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

        private async Task<IEnumerable<Lap>> GetLapsAsync(long activityId)
        {
            IEnumerable<Lap> laps;

            var json = await GetDataAsync(string.Format(Constants.LapsPartUrl, activityId));
            laps = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<Lap>>(json));

            return laps;
        }

        public async Task<Activity> GetLatestAsync()
        {
            var latestJson = await GetDataAsync(Constants.ActivityLastestSummaryPartUrl);
            var latest = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<ActivitySummary>>(latestJson));

            await CreateOrUpdateActivitySummaryAsync(latest);

            var id = latest.OrderByDescending(m => m.StartDate).FirstOrDefault().Id;
            var json = await GetDataAsync(string.Format(Constants.ActivityPartUrl, id));
            var activity = Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Activity>(json));
            activity.Result.Laps = await GetLapsAsync(activity.Id);
            await CreateOrUpdateActivityAsync(new List<Activity> { activity.Result });

            return await activity;
        }

        private async Task<string> GetDataAsync(string url)
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
}
