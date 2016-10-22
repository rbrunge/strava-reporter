using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StravaReporter.Models.Strava
{

    public class Achievement
    {

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }
    }

}