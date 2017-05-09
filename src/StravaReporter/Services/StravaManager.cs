using Newtonsoft.Json;
using StravaReporter.Models.Strava;
using StravaReporter.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StravaReporter.Services
{


    public class StravaManager : IStravaManager
    {
        private readonly IStravaConnector _stravaConnector;
        private readonly ICacheRepository _cacheRepository;

        public StravaManager(IStravaConnector stravaConnector, ICacheRepository cacheRepository)
        {
            _stravaConnector = stravaConnector;
            _cacheRepository = cacheRepository;
        }

        public async Task<Activity> GetLatestAsync()
        {
            var latestJson = await _stravaConnector.GetDataAsync(Constants.ActivityLastestSummaryPartUrl);
            var latest = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<ActivitySummary>>(latestJson));

            await _cacheRepository.CreateOrUpdateActivitySummaryAsync(latest);

            var id = latest.OrderByDescending(m => m.StartDate).FirstOrDefault().Id;
            var activity = ReadThrough<Activity>(
                cache: async () => await _cacheRepository.GetActivityAsync(id),
                remote: async () =>
                {
                    var json = await _stravaConnector.GetDataAsync(
                        string.Format(Constants.ActivityPartUrl, id));
                    var a = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Activity>(json));
                    a.Laps = await GetLapsAsync(a.Id);
                    await _cacheRepository.CreateOrUpdateActivityAsync(new List<Activity> { a });
                    return a;
                });

            return activity;
        }

        private T ReadThrough<T>(Func<Task<T>> cache, Func<Task<T>> remote) where T: class
        {
            T t = (T)((object)cache().Result);
            if (t == null)
            {
                t = remote().Result;
            }
            return t;
        }


        private async Task<IEnumerable<Lap>> GetLapsAsync(long activityId)
        {
            IEnumerable<Lap> laps;

            var json = await _stravaConnector.GetDataAsync(string.Format(Constants.LapsPartUrl, activityId));
            laps = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<Lap>>(json));

            return laps;
        }
    }
}
