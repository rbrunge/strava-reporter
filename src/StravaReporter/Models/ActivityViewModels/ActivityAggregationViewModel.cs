using System;
using StravaReporter.Models.Strava;
using System.Collections.Generic;

namespace StravaReporter.Models.ActivityViewModels
{
    public class ActivityAggregationViewModel
    {
        public Activity Activity { get; set; }

        public string ActivityIcon
        {
            get
            {
                if (Activity == null) throw new ArgumentNullException(nameof(Activity));
                var activityIcon =
                    Activity.Type == "Ride"
                        ? "/images/icons/cycling.png"
                        : Activity.Type == "Swim"
                            ? "/images/icons/swimming.png"
                            : Activity.Type == "Run"
                                ? "/images/icons/running.png"
                                : "";
                return activityIcon;
            }
        }

        public string ToPace(double distance, double time)
        {
            if (Activity == null) throw new ArgumentNullException(nameof(Activity));
            switch (Activity.Type)
            {
                case "Swim":
                    return ToSwimPace(distance, time);
                case "Run":
                    return ToRunPace(distance, time);
                case "Ride":
                default:
                    return ToSpeed(distance, time);
            }
        }

        private string ToRunPace(double distance, double time)
        {
            return TimeSpan.FromSeconds(1000 / (distance / time)).ToString(@"mm\:ss") + " min/km";
        }
        private string ToSwimPace(double distance, double time)
        {
            return TimeSpan.FromSeconds(100 / (distance / time)).ToString(@"mm\:ss") + " min/100 m";
        }
        private string ToSpeed(double distance, double time)
        {
            return distance / time * 3.6 + " km/h";
        }
        public string FormattedCadence(double cadence)
        {
            return (cadence < 125 ? cadence * 2 : cadence).ToString("N0");
        }
        public string FormattedTime(double time)
        {
            return TimeSpan.FromSeconds(time).ToString(time < 3600 ? @"mm\:ss" : @"hh\:mm\:ss");
        }
        public string FormattedDistance(double distance)
        {
            return Math.Round(distance / 1000, 1).ToString("N1") + " km";
        }


    }
}
