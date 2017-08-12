using System;
using System.Collections.Generic;
using System.Linq;
using StravaReporter.Models.Strava;
using System.Threading.Tasks;
using StravaReporter.Integration;
using StravaReporter.Repositories;

namespace StravaReporter.Services
{

    public class ActivityService : IActivityService
    {
        private readonly IStravaIntegrator _stravaIntegrator;
        private readonly IActivityRepository _activityRepository;

        public ActivityService(IStravaIntegrator stravaIntegrator, IActivityRepository activityRepository)
        {
            _stravaIntegrator = stravaIntegrator;
            _activityRepository = activityRepository;
        }

        public async Task<Activity> GetLatestAsync()
        {
            if (_stravaIntegrator == null) throw new ArgumentNullException(nameof(_stravaIntegrator));
            var latestActivityAsync = _stravaIntegrator.GetLatestActivityAsync();
            if (latestActivityAsync != null)
            {
                var activity = await latestActivityAsync;

                //await _activityRepository.CreateOrUpdateActivitySummaryAsync(latest);

                //var id = latest.OrderByDescending(m => m.StartDate).FirstOrDefault().Id;
                //var activity = ReadThrough<Activity>(
                //    repository: async () => await _cacheRepository.GetActivityAsync(id),
                //    remote: async () =>
                //    {
                //        var json = await _stravaConnector.GetDataAsync(
                //            string.Format(Constants.ActivityPartUrl, id));
                //        var a = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Activity>(json));
                //        a.Laps = await GetLapsAsync(a.Id);
                //        await _cacheRepository.CreateOrUpdateActivityAsync(new List<Activity> { a });
                //        return a;
                //    });

                return activity;
            }
            return null;
        }

        //private T ReadThrough<T>(Func<Task<T>> repository, Func<Task<T>> remote) where T : class
        //{
        //    return ReadThrough(null, repository, remote);
        //}

        //private T ReadThrough<T>(string key, Func<Task<T>> getStorage, Func<Task<T>> setStorage) where T : class
        //{
        //    Task<T> task;
        //    if (storage != null)
        //    {
        //        task = cache();
        //        T t = (T) (object) task?.Result;
        //        if (t != null) return t;
        //    }
        //    if (repository != null)
        //    {
        //        task = repository();
        //        T t = (T) (object) task?.Result;
        //        if (t != null) return t;
        //    }

        //    task = remote?.Invoke();
        //    return task?.Result;
        //}
    }
}