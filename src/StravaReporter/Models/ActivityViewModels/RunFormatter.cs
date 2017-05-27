using System;

namespace StravaReporter.Models.ActivityViewModels
{
    public class RunFormatter : ActivityFormatter
    {
        public override string Icon { get; } = "/images/icons/running.png";
        public override string UnitOfSpeed { get; } = "min/km";
        public override string UnitOfMeasurement { get; } = "km";
        public override string NameForSpeedOrPace { get; } = "Pace";
        public override string ToPace(double distance, double time)
        {
            return string.Format("{0} {1}",
                TimeSpan.FromSeconds(1000 / (distance / time)).ToString(@"mm\:ss"),
                UnitOfSpeed);
        }
        public override string FormatDistance(double distanceInMeters)
        {
            return string.Format("{0} {1}", 
                Math.Round(distanceInMeters / 1000, 1).ToString("N1"),
                UnitOfMeasurement);
        }
    }
}