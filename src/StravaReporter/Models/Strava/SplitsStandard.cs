using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StravaReporter.Models.Strava
{

    public class SplitsStandard
    {

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("elapsed_time")]
        public double ElapsedTime { get; set; }

        [JsonProperty("elevation_difference")]
        public double ElevationDifference { get; set; }

        [JsonProperty("moving_time")]
        public double MovingTime { get; set; }

        [JsonProperty("split")]
        public double Split { get; set; }

        [JsonProperty("average_heartrate")]
        public double AverageHeartrate { get; set; }
    }

}