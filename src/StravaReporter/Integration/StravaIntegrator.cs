using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StravaReporter.Models.Strava;
using StravaReporter.Repositories;
using StravaReporter.Services;

namespace StravaReporter.Integration
{


    public class StravaIntegrator : IStravaIntegrator
    {
        public class Constants
        {
            public const string AccessToken = "urn:strava:accesstoken";
            public static readonly Uri ApiBaseUrl = new Uri("https://www.strava.com/api/v3/");
            public static readonly Uri WebBaseUrl = new Uri("https://www.strava.com/");
            public const string ActivityLastestSummaryPartUrl = "athlete/activities?per_page=1&page=1";
            public const string ActivityPartUrl = "activities/{0}";
            public const string FlyByUrl = "http://labs.strava.com/flyby/viewer/#{0}";
            public const string LapsPartUrl = "activities/{0}/laps";
        }

        private readonly IAccessTokenProvider _tokenProvider;

        public StravaIntegrator(IAccessTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public async Task<Activity> GetLatestActivityAsync()
        {
            var latestJson = await GetDataAsync(Constants.ActivityLastestSummaryPartUrl);
            var latest = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<ActivitySummary>>(latestJson));
            var id = latest.OrderByDescending(m => m.StartDate).FirstOrDefault().Id;
            var json = await GetDataAsync(string.Format(Constants.ActivityPartUrl, id));
            var activity = Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Activity>(json));
            activity.Result.Laps = await GetLapsAsync(activity.Id);
            return await activity;
        }

        private async Task<IEnumerable<Lap>> GetLapsAsync(long activityId)
        {
            IEnumerable<Lap> laps;

            var json = await GetDataAsync(string.Format(Constants.LapsPartUrl, activityId));
            laps = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<Lap>>(json));

            return laps;
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

        private async Task<string> GetDataAsync(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = Constants.ApiBaseUrl;
                if (client.DefaultRequestHeaders != null)
                    if (_tokenProvider != null)
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _tokenProvider.Token);
                var stringAsync = client.GetStringAsync(url);
                if (stringAsync != null)
                {
                    var json = await stringAsync;
                    return json;
                }
            }
            return null;
        }
    }
}
