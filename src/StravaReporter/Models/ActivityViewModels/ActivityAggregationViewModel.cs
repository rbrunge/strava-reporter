using System;
using System.Linq;
using Microsoft.Extensions.Options;
using Nest;
using StravaReporter.Models.Strava;

namespace StravaReporter.Models.ActivityViewModels
{
    public interface IActivityAggregationViewModel
    {
        Activity Activity { get; set; }
        string StravaActivityLink { get; }
        string StravaUrlFlyby { get; }
        bool ShowSplits { get; }
        IActivityFormatter Pace { get; }
    }

    public class ActivityAggregationViewModel : IActivityAggregationViewModel
    {
        public Activity Activity
        {
            get { return _activity; }
            set
            {
                _activity = value;
                StravaActivityLink =
                    string.Format(_appkeys?.StravaUrlWebbase + _appkeys?.StravaUrlActivityPartUrl, _activity.Id);
                StravaUrlFlyby = 
                    string.Format(_appkeys?.StravaUrlFlyby, _activity.Id);
            }
        }

        public string StravaActivityLink { get; private set; }
        public string StravaUrlFlyby { get; private set; }

        private AppKeyConfig _appkeys;
        private Activity _activity;

        public ActivityAggregationViewModel(IOptions<AppKeyConfig> appkeys)
        {
            _appkeys = appkeys.Value;
            if (_appkeys == null) throw new ArgumentException(nameof(_appkeys));
        }

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
