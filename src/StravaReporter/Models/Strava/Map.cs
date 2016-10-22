using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StravaReporter.Models.Strava
{

    public class Map
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("polyline")]
        public string Polyline { get; set; }

        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        [JsonProperty("summary_polyline")]
        public string SummaryPolyline { get; set; }
    }

}