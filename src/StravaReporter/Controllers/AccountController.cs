using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StravaReporter.Services;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace StravaReporter.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly IUserSessionStateManager _userSessionState;

        public AccountController(
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory, 
            IUserSessionStateManager userSessionState)
        {
            _emailSender = emailSender;
            _smsSender = smsSender;
            _userSessionState = userSessionState;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        public IActionResult UserProfile()
        {
            return View();
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogOff()
        {
            _logger.LogInformation(4, "User logged out.");
            ControllerContext.HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(WelcomeController.Index), "Welcome");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("Index", "Activity", new { ReturnUrl = returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            var challenge = Challenge(properties, provider);
            _userSessionState.Current.LastFetchDateTime = DateTime.Now;
            _userSessionState.Current.Test = "Hello";
            return challenge;
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(WelcomeController.Index), "Welcome");
            }
        }

        #endregion
    }
}
