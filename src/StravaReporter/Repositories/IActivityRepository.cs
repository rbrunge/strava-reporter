using System.Collections.Generic;
using System.Threading.Tasks;
using StravaReporter.Models.Strava;

namespace StravaReporter.Repositories
{
    public interface IActivityRepository
    {
        Task<ActivitySummary> GetActivitySummaryAsync(long id);
        Task CreateOrUpdateActivitySummaryAsync(IEnumerable<ActivitySummary> activitySummary);
        Task<Activity> GetActivityAsync(long id);
        Task CreateOrUpdateActivityAsync(IEnumerable<Activity> activitySummary);
    }
}