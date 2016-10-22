using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StravaReporter.Models.Strava
{

    public class Gear
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("primary")]
        public bool Primary { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }
    }

}