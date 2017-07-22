using System;

namespace StravaReporter.Models.ActivityViewModels
{
    public class SwimFormatter : ActivityFormatter
    {
        public override string Icon { get; } = "/images/icons/swimming.png";
        public override string UnitOfSpeed { get; } = "min/100 m";
        public override string UnitOfMeasurement { get; } = "km";
        public override string NameForSpeedOrPace { get; } = "Pace";
        public override string ToPace(double distance, double time)
        {
            return string.Format("{0} {1}",
                TimeSpan.FromSeconds(100 / (distance / time)).ToString(@"mm\:ss"),
                UnitOfSpeed);
        }
    }
}