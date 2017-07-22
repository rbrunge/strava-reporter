using System;

namespace StravaReporter.Services
{
    public interface IActivityState
    {
        DateTime LastFetchDateTime { get; set; }
    }
}