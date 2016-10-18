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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IStravaManager _stravaManager;

        public ActivityController(UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory, IStravaManager stravaManager)
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _stravaManager = stravaManager;
        }

        public async Task<IActionResult> Index()
        {
            // var token = User.Claims.FirstOrDefault(n => n.Type == Constants.AccessToken).Value;
            var model = new ActivityAggregationViewModel();
            model.Activity = await _stravaManager.GetLatestAsync();
            model.Laps = await _stravaManager.GetLapsAsync(model.Activity.Id);
            return View(model);
        }
    }
}