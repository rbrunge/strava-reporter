using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using StravaReporter.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using StravaReporter.Services;
using StravaReporter.Models.ActivityViewModels;

namespace StravaReporter.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private readonly ILogger _logger;
        private readonly IActivityService _activityService;

        public ActivityController(ILoggerFactory loggerFactory, IActivityService activityService)
        {
            _activityService = activityService;
        }

        public async Task<IActionResult> Index()
        {
            if (_activityService == null) throw new ArgumentNullException(nameof(_activityService));
            var model = new ActivityAggregationViewModel();
            var latestAsync = _activityService.GetLatestAsync();
            if (latestAsync != null) model.Activity = await latestAsync;
            return View(model);
        }
    }
}