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

    public interface IAccessTokenProvider
    {
        string Token { get; }
    }

}