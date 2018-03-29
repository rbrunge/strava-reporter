using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StravaReporter.Models.Strava
{

    public class Athlete
    {

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }
    }

}