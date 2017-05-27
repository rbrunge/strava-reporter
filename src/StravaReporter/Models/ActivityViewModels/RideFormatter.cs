using System;

namespace StravaReporter.Models.ActivityViewModels
{
    public class RideFormatter : ActivityFormatter
    {
        public override string Icon { get; } = "/images/icons/cycling.png";
        public override string UnitOfSpeed { get; } = "km/h";
        public override string UnitOfMeasurement { get; } = "km";
        public override string NameForSpeedOrPace { get; } = "Speed";
        public override string ToPace(double distance, double time)
        {
            return string.Format("{0} {1}",
                (distance / time * 3.6).ToString("N1"),
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