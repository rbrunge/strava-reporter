using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using StravaReporter.Models.Strava;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StravaReporter.Services
{

    public class HttpContextAccessTokenProvider : IAccessTokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ClaimsPrincipal _caller;
        public HttpContextAccessTokenProvider(IHttpContextAccessor contextAccessor, ClaimsPrincipal caller)
        {
            _contextAccessor = contextAccessor;
            _caller = caller;
        }
        public string Token => _caller.Claims.FirstOrDefault(c => c.Type == "urn:strava:accesstoken").Value;
    }

}