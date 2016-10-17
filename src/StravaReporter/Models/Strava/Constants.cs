using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StravaReporter.Models.Strava
{
    public class Constants
    {
        public static readonly Uri BaseUrl = new Uri("https://www.strava.com/api/v3/");
        public const string ActivityLastestSummaryUrl = "athlete/activities?per_page=1&page=1";
        public const string ActivityUrl = "activities/{0}";
    }
}
