using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StravaReporter.Models.Strava
{

    public class Trend
    {

        [JsonProperty("speeds")]
        public IList<double> Speeds { get; set; }

        [JsonProperty("current_activity_index")]
        public int CurrentActivityIndex { get; set; }

        [JsonProperty("min_speed")]
        public double MinSpeed { get; set; }

        [JsonProperty("mid_speed")]
        public double MidSpeed { get; set; }

        [JsonProperty("max_speed")]
        public double MaxSpeed { get; set; }

        [JsonProperty("direction")]
        public int Direction { get; set; }
    }

}