using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StravaReporter.Models.Strava
{

    public class Primary
    {

        [JsonProperty("id")]
        public object Id { get; set; }

        [JsonProperty("unique_id")]
        public string UniqueId { get; set; }

        //[JsonProperty("urls")]
        //public Urls Urls { get; set; }

        [JsonProperty("source")]
        public int Source { get; set; }
    }

}