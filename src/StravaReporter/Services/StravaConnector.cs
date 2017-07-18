using System;
using System.Collections.Generic;
using System.Linq;
using StravaReporter.Models.Strava;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StravaReporter.Repositories;

namespace StravaReporter.Services
{

    public class StravaConnector : IStravaConnector
    {
        private readonly IActivityRepository _activityRepository;
        public StravaConnector(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }



        private T ReadThrough<T>(Func<Task<T>> cache, Func<Task<T>> remote) where T : class
        {
            T t = (T)((object)cache().Result);
            if (t == null)
            {
                t = remote().Result;
            }
            return t;
        }

    }

}