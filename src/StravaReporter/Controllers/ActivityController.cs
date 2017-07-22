using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using StravaReporter.Services;
using StravaReporter.Models.ActivityViewModels;

namespace StravaReporter.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly IActivityAggregationViewModel _activityAggregationViewModel;

        public ActivityController(
            ILoggerFactory loggerFactory, 
            IActivityService activityService, 
            IActivityAggregationViewModel activityAggregationViewModel)
        {
            _activityService = activityService;
            _activityAggregationViewModel = activityAggregationViewModel;
        }

        public async Task<IActionResult> Index()
        {
            if (_activityService == null) throw new ArgumentNullException(nameof(_activityService));
            var latestAsync = _activityService.GetLatestAsync();
            if (latestAsync != null) _activityAggregationViewModel.Activity = await latestAsync;
            return View(_activityAggregationViewModel);
        }
    }
}