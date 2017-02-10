using Microsoft.Extensions.Options;
using Nest;
using StravaReporter.Models;
using StravaReporter.Models.Strava;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StravaReporter.Repositories
{
    public interface IRemoteRepository
    {
        Task<ActivitySummary> GetActivitySummary(int id);
        void CreateOrUpdateActivitySummary(IEnumerable<ActivitySummary> activitySummary);
    }

    public class RemoteRepository : IRemoteRepository
    {
        private readonly ElasticsearchSettings _settings;
        private readonly IElasticClient _elasticClient;

        public RemoteRepository(IOptions<ElasticsearchSettings> settings, IElasticClient elasticClient)
        {
            _settings = settings.Value;
            if (_settings == null) throw new ArgumentNullException(nameof(_settings));
            if (string.IsNullOrEmpty(_settings.FullAccessUrl)) throw new ArgumentNullException(nameof(_settings.FullAccessUrl));
            //_elasticClient = elasticClient;
            //_elasticClient = new ConnectionSettings(new Uri(_settings.FullAccessUrl));
        }

        public async void CreateOrUpdateActivitySummary(IEnumerable<ActivitySummary> activitySummary)
        {
            var node = new Uri(_settings.FullAccessUrl);
            var settings = new ConnectionSettings(node);
            var client = new ElasticClient(settings);
            var response = await client.IndexManyAsync(activitySummary,  nameof(ActivitySummary).ToLowerInvariant());
        }

        public async Task<ActivitySummary> GetActivitySummary(int id)
        {
            var node = new Uri(_settings.FullAccessUrl);
            var settings = new ConnectionSettings(node);
            var client = new ElasticClient(settings);
            var response = await client.GetAsync(new DocumentPath<ActivitySummary>(id));
            return response.Source;
        }
    }
}
