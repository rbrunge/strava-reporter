using System;

namespace StravaReporter.Models.Strava
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
}
