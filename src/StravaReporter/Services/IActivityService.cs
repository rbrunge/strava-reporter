using System.Threading.Tasks;
using StravaReporter.Models.Strava;

namespace StravaReporter.Services
{
    public interface IActivityService
    {
        Task<Activity> GetLatestAsync();
    }

}