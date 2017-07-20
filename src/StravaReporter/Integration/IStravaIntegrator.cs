using System.Collections.Generic;
using System.Threading.Tasks;
using StravaReporter.Models.Strava;

namespace StravaReporter.Integration
{
    public interface IStravaIntegrator
    {
        Task<Activity> GetLatestActivityAsync();
        // Task<Activity> GetLatestActivityIdAsync();
        // Task<IEnumerable<Lap>> GetLapsAsync(long activityId);
    }
}