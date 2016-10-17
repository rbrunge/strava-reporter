using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using StravaReporter.Models.Strava;

namespace StravaReporter.Controllers
{
    public class BestEffortController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var token = User.Claims.FirstOrDefault(n => n.Type == "urn:strava:accesstoken").Value;
            var activity = await Activity.GetLatest(token);
            // var result = 
            // return Content(JsonConvert.SerializeObject(activity, Formatting.Indented));
            return View(activity);
        }
    }
}