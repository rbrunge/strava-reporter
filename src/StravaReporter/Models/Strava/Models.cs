using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StravaReporter.Models.Strava
{
    public class Athlete
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }
    }

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

    public class Segment
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("activity_type")]
        public string ActivityType { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("average_grade")]
        public double AverageGrade { get; set; }

        [JsonProperty("maximum_grade")]
        public double MaximumGrade { get; set; }

        [JsonProperty("elevation_high")]
        public double ElevationHigh { get; set; }

        [JsonProperty("elevation_low")]
        public double ElevationLow { get; set; }

        [JsonProperty("start_latlng")]
        public IList<double> StartLatlng { get; set; }

        [JsonProperty("end_latlng")]
        public IList<double> EndLatlng { get; set; }

        [JsonProperty("start_latitude")]
        public double StartLatitude { get; set; }

        [JsonProperty("start_longitude")]
        public double StartLongitude { get; set; }

        [JsonProperty("end_latitude")]
        public double EndLatitude { get; set; }

        [JsonProperty("end_longitude")]
        public double EndLongitude { get; set; }

        [JsonProperty("climb_category")]
        public int ClimbCategory { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("private")]
        public bool Private { get; set; }

        [JsonProperty("hazardous")]
        public bool Hazardous { get; set; }

        [JsonProperty("starred")]
        public bool Starred { get; set; }
    }

    public class Achievement
    {

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }
    }

    public class SegmentEffort
    {

        [JsonProperty("id")]
        public object Id { get; set; }

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

        [JsonProperty("average_cadence")]
        public double AverageCadence { get; set; }

        [JsonProperty("average_heartrate")]
        public double AverageHeartrate { get; set; }

        [JsonProperty("max_heartrate")]
        public double MaxHeartrate { get; set; }

        [JsonProperty("segment")]
        public Segment Segment { get; set; }

        [JsonProperty("kom_rank")]
        public object KomRank { get; set; }

        [JsonProperty("pr_rank")]
        public int? PrRank { get; set; }

        [JsonProperty("achievements")]
        public IList<Achievement> Achievements { get; set; }

        [JsonProperty("hidden")]
        public bool Hidden { get; set; }
    }

    public class SplitsMetric
    {

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("elapsed_time")]
        public double ElapsedTime { get; set; }

        [JsonProperty("elevation_difference")]
        public double ElevationDifference { get; set; }

        [JsonProperty("moving_time")]
        public double MovingTime { get; set; }

        [JsonProperty("split")]
        public double Split { get; set; }

        [JsonProperty("average_heartrate")]
        public double AverageHeartrate { get; set; }
    }

    public class SplitsStandard
    {

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("elapsed_time")]
        public double ElapsedTime { get; set; }

        [JsonProperty("elevation_difference")]
        public double ElevationDifference { get; set; }

        [JsonProperty("moving_time")]
        public double MovingTime { get; set; }

        [JsonProperty("split")]
        public double Split { get; set; }

        [JsonProperty("average_heartrate")]
        public double AverageHeartrate { get; set; }
    }

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

    public class Photos
    {

        [JsonProperty("primary")]
        public Primary Primary { get; set; }

        [JsonProperty("use_primary_photo")]
        public bool UsePrimaryPhoto { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }

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

    public class SimilarActivities
    {

        [JsonProperty("effort_count")]
        public int EffortCount { get; set; }

        [JsonProperty("average_speed")]
        public double AverageSpeed { get; set; }

        [JsonProperty("min_average_speed")]
        public double MinAverageSpeed { get; set; }

        [JsonProperty("mid_average_speed")]
        public double MidAverageSpeed { get; set; }

        [JsonProperty("max_average_speed")]
        public double MaxAverageSpeed { get; set; }

        [JsonProperty("pr_rank")]
        public object PrRank { get; set; }

        [JsonProperty("frequency_milestone")]
        public object FrequencyMilestone { get; set; }

        [JsonProperty("trend")]
        public Trend Trend { get; set; }

        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }
    }

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
    }

    public class Lap
    {
        [JsonProperty("id")]
        public object Id { get; set; }

        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("activity")]
        public Activity Activity { get; set; }

        [JsonProperty("athlete")]
        public Athlete Athlete { get; set; }

        [JsonProperty("elapsed_time")]
        public int ElapsedTime { get; set; }

        [JsonProperty("moving_time")]
        public int MovingTime { get; set; }

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

        [JsonProperty("total_elevation_gain")]
        public double TotalElevationGain { get; set; }

        [JsonProperty("average_speed")]
        public double AverageSpeed { get; set; }

        [JsonProperty("max_speed")]
        public double MaxSpeed { get; set; }

        [JsonProperty("average_cadence")]
        public double AverageCadence { get; set; }

        [JsonProperty("average_heartrate")]
        public double AverageHeartrate { get; set; }

        [JsonProperty("max_heartrate")]
        public double MaxHeartrate { get; set; }

        [JsonProperty("lap_index")]
        public int LapIndex { get; set; }
    }

}