using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StravaReporter.Models.Strava
{

    public class ActivitySummary
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

        [JsonProperty("upload_id")]
        public int UploadId { get; set; }

        [JsonProperty("athlete")]
        public Athlete Athlete { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("moving_time")]
        public int MovingTime { get; set; }

        [JsonProperty("elapsed_time")]
        public int ElapsedTime { get; set; }

        [JsonProperty("total_elevation_gain")]
        public double TotalElevationGain { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("start_date_local")]
        public DateTime StartDateLocal { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("start_latlng")]
        public IList<double> StartLatlng { get; set; }

        [JsonProperty("end_latlng")]
        public IList<double> EndLatlng { get; set; }

        [JsonProperty("location_city")]
        public string LocationCity { get; set; }

        [JsonProperty("location_state")]
        public string LocationState { get; set; }

        [JsonProperty("location_country")]
        public string LocationCountry { get; set; }

        [JsonProperty("start_latitude")]
        public double StartLatitude { get; set; }

        [JsonProperty("start_longitude")]
        public double StartLongitude { get; set; }

        [JsonProperty("achievement_count")]
        public int AchievementCount { get; set; }

        [JsonProperty("kudos_count")]
        public int KudosCount { get; set; }

        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }

        [JsonProperty("athlete_count")]
        public int AthleteCount { get; set; }

        [JsonProperty("photo_count")]
        public int PhotoCount { get; set; }

        [JsonProperty("map")]
        public Map Map { get; set; }

        [JsonProperty("trainer")]
        public bool Trainer { get; set; }

        [JsonProperty("commute")]
        public bool Commute { get; set; }

        [JsonProperty("manual")]
        public bool Manual { get; set; }

        [JsonProperty("private")]
        public bool Private { get; set; }

        [JsonProperty("flagged")]
        public bool Flagged { get; set; }

        [JsonProperty("gear_id")]
        public string GearId { get; set; }

        [JsonProperty("average_speed")]
        public double AverageSpeed { get; set; }

        [JsonProperty("max_speed")]
        public double MaxSpeed { get; set; }

        [JsonProperty("average_cadence")]
        public double AverageCadence { get; set; }

        [JsonProperty("average_temp")]
        public double AverageTemp { get; set; }

        [JsonProperty("has_heartrate")]
        public bool HasHeartrate { get; set; }

        [JsonProperty("average_heartrate")]
        public double AverageHeartrate { get; set; }

        [JsonProperty("max_heartrate")]
        public double MaxHeartrate { get; set; }

        [JsonProperty("elev_high")]
        public double ElevHigh { get; set; }

        [JsonProperty("elev_low")]
        public double ElevLow { get; set; }

        [JsonProperty("total_photo_count")]
        public int TotalPhotoCount { get; set; }

        [JsonProperty("has_kudoed")]
        public bool HasKudoed { get; set; }

        [JsonProperty("workout_type")]
        public int? WorkoutType { get; set; }
    }

}