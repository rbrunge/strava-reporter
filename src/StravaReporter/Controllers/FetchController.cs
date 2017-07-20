using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StravaReporter.Models;
using StravaReporter.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace StravaReporter.Controllers
{
    [Authorize]
    public class FetchController : Controller
    {

        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = new FetchViewModel
            {
                StartDate = DateTime.Today.AddMonths(-1)
            };

            return View(model);
        }
        // GET: /<controller>/
        [HttpPost]
        public async Task<IActionResult> Index(FetchViewModel model)
        {
            // var v = await _stravaConnector.


            return View(model);
        }
    }
}
