using Newtonsoft.Json;
using StravaReporter.Models.Strava;
using StravaReporter.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StravaReporter.Services
{


    public class StravaManager : IStravaManager
    {
        private readonly IStravaConnector _stravaConnector;
        private readonly IRemoteRepository _remoteRepository;

        public StravaManager(IStravaConnector stravaConnector, IRemoteRepository remoteRepository)
        {
            _stravaConnector = stravaConnector;
            _remoteRepository = remoteRepository;
        }

        public async Task<Activity> GetLatestAsync()
        {
            Activity activity = null;

            var latestJson = await _stravaConnector.GetDataAsync(Constants.ActivityLastestSummaryPartUrl);
            var latest = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<ActivitySummary>>(latestJson));

            _remoteRepository.CreateOrUpdateActivitySummary(latest);

            var json = await _stravaConnector.GetDataAsync(
                string.Format(Constants.ActivityPartUrl, latest.OrderByDescending(m => m.StartDate).FirstOrDefault().Id));
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
