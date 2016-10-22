using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StravaReporter.Models.Strava
{

    public class Activity : ActivitySummary
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("calories")]
        public double Calories { get; set; }

        [JsonProperty("segment_efforts")]
        public IList<SegmentEffort> SegmentEfforts { get; set; }

        [JsonProperty("splits_metric")]
        public IList<SplitsMetric> SplitsMetric { get; set; }

        [JsonProperty("splits_standard")]
        public IList<SplitsStandard> SplitsStandard { get; set; }

        [JsonProperty("best_efforts")]
        public IList<BestEffort> BestEfforts { get; set; }

        [JsonProperty("gear")]
        public Gear Gear { get; set; }

        [JsonProperty("partner_logo_url")]
        public object PartnerLogoUrl { get; set; }

        [JsonProperty("photos")]
        public Photos Photos { get; set; }

        [JsonProperty("similar_activities")]
        public SimilarActivities SimilarActivities { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("embed_token")]
        public string EmbedToken { get; set; }

        [JsonProperty("laps")]
        public IEnumerable<Lap> Laps { get; set; }
    }

}