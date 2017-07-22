using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using StravaReporter.Services;

namespace StravaReporter.Controllers
{
    [AllowAnonymous]
    public class WelcomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUserSessionStateManager _userSessionState;

        public WelcomeController(ILoggerFactory loggerFactory, IUserSessionStateManager userSessionState)
        {
            _userSessionState = userSessionState;
            _logger = loggerFactory.CreateLogger<WelcomeController>();
        }

        public IActionResult Index()
        {
            var test = _userSessionState.Current;
            return View();
        }
    }
}