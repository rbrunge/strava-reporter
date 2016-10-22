using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StravaReporter.Models.Strava
{

    public class BestEffort
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("activity")]
        public Activity Activity { get; set; }

        [JsonProperty("athlete")]
        public Athlete Athlete { get; set; }

        [JsonProperty("elapsed_time")]
        public double ElapsedTime { get; set; }

        [JsonProperty("moving_time")]
        public double MovingTime { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("start_date_local")]
        public DateTime StartDateLocal { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("start_index")]
        public int StartIndex { get; set; }

        [JsonProperty("end_index")]
        public int EndIndex { get; set; }

        [JsonProperty("pr_rank")]
        public object PrRank { get; set; }

        [JsonProperty("achievements")]
        public IList<object> Achievements { get; set; }
    }

}