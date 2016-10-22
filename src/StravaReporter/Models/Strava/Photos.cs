using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StravaReporter.Models.Strava
{

    public class Photos
    {

        [JsonProperty("primary")]
        public Primary Primary { get; set; }

        [JsonProperty("use_primary_photo")]
        public bool UsePrimaryPhoto { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }

}