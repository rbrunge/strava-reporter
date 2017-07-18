using System.Collections.Generic;
using System.Threading.Tasks;
using StravaReporter.Models.Strava;

namespace StravaReporter.Services
{

    public interface IStravaConnector
    {
        Task<Activity> GetLatestAsync();
    }

}