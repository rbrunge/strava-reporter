using System;
using System.Linq;
using StravaReporter.Models.Strava;

namespace StravaReporter.Models.ActivityViewModels
{
    public class ActivityAggregationViewModel
    {
        public Activity Activity { get; set; }

        public bool ShowSplits
        {
            get
            {
                if (Activity == null) throw new ArgumentNullException(nameof(Activity));
                return Activity.SplitsMetric.Any() && Activity.Type != "Swim";
            }
        }

        public IActivityFormatter Pace
        {
            get
            {
                if (Activity == null) throw new ArgumentNullException(nameof(Activity));
                switch (Activity.Type)
                {
                    case "Swim":
                        return new SwinFormatter();
                    case "Run":
                        return new RunFormatter();
                    case "Ride":
                    default:
                        return new RideFormatter();
                }
            }
        }
    }
}
