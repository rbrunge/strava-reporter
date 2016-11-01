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

            var latestJson = await _stravaConnector.GetDataAsync(Constants.ActivityLastestSummaryPartUrl);
            var latest = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<ActivitySummary>>(latestJson));
            var json = await _stravaConnector.GetDataAsync(string.Format(Constants.ActivityPartUrl, latest.FirstOrDefault().Id));
            activity = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Activity>(json));

            return activity;
        }
        public async Task<IEnumerable<Lap>> GetLapsAsync(int activityId)
        {
            IEnumerable<Lap> laps;

            var json = await _stravaConnector.GetDataAsync(string.Format(Constants.LapsPartUrl, activityId));
            laps = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<Lap>>(json));

            return laps;
        }
    }
}
