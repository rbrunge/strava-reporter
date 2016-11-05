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
    [AllowAnonymous]
    public class WelcomeController : Controller
    {
        private readonly ILogger _logger;

        public WelcomeController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WelcomeController>();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}