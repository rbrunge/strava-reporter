using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StravaReporter.Models.Strava;
using Microsoft.AspNetCore.Identity;
using StravaReporter.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace StravaReporter.Controllers
{
    [Authorize]
    public class BestEffortController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public BestEffortController(UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<ManageController>();
        }

        public async Task<IActionResult> Index()
        {
            var token = User.Claims.FirstOrDefault(n => n.Type == Constants.AccessToken).Value;
            var activity = await Activity.GetLatestAsync(token);
            return View(activity);
        }
    }
}