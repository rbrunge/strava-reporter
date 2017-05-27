using System;

namespace StravaReporter.Models.ActivityViewModels
{
    public abstract class ActivityFormatter : IActivityFormatter
    {
        public abstract string Icon { get; }
        public abstract string UnitOfSpeed { get; }
        public abstract string UnitOfMeasurement { get; }
        public virtual string UnitOfTime { get; } = "min";
        public abstract string NameForSpeedOrPace { get; }
        public abstract string ToPace(double distance, double time);
        public virtual string FormatCadence(double cadence)
        {
            return (cadence < 125 ? cadence * 2 : cadence).ToString("N0");
        }
        public virtual string FormatTime(double timeInSeconds)
        {
            return TimeSpan.FromSeconds(timeInSeconds).ToString(timeInSeconds < 3600 ? @"mm\:ss" : @"hh\:mm\:ss");
        }
        public virtual string FormatDistance(double distanceInMeters)
        {
            return string.Format("{0} {1}",
                Math.Round(distanceInMeters / 1000, 1).ToString("N1"),
                UnitOfMeasurement);
        }
    }
}