using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using StravaReporter.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace StravaReporter
{
    public class StravaOptions : OAuthOptions
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public StravaOptions()
        {
        }
        public StravaOptions(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
    }
    
    public partial class Startup
    {

        private StravaOptions StravaOptions
        {
            get
            {
                var options = new StravaOptions
                {
                    DisplayName = "Strava",
                    ClientId = Configuration["Authentication:Strava:ClientId"],
                    ClientSecret = Configuration["Authentication:Strava:ClientSecret"],
                    TokenEndpoint = "https://www.strava.com/oauth/token",
                    AuthorizationEndpoint = "https://www.strava.com/oauth/authorize",
                    UserInformationEndpoint = "https://www.strava.com/api/v3/athlete",
                    AuthenticationScheme = "Strava",
                    CallbackPath = new PathString("/signin-strava"),
                    SaveTokens = true,
                    Events = new OAuthEvents
                    {
                        OnCreatingTicket = async context => { await CreatingStravaAuthTicket(context); },
                        OnTicketReceived = async context => { await StravaTicketReceived(context); },
                    }
                };
                options.Scope.Add("view_private");
                return options;
            }
        }

        /// <summary>
        /// Creates tickets and add claims. Claims are "fetched" via signin later.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static async Task CreatingStravaAuthTicket(OAuthCreatingTicketContext context)
        {
            // Get the GitHub user
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            var user = JObject.Parse(await response.Content.ReadAsStringAsync());
            context.Identity.AddClaim(new Claim(Models.Strava.Constants.AccessToken, context.AccessToken));
            var identifier = user.Value<string>("id");
            if (!string.IsNullOrEmpty(identifier))
            {
                context.Identity.AddClaim(new Claim(
                    ClaimTypes.NameIdentifier, identifier,
                    ClaimValueTypes.String, context.Options.ClaimsIssuer));
            }

            var userName = user.Value<string>("username");
            if (!string.IsNullOrEmpty(userName))
            {
                context.Identity.AddClaim(new Claim(
                    ClaimsIdentity.DefaultNameClaimType, userName,
                    ClaimValueTypes.String, context.Options.ClaimsIssuer));
            }

            var email = user.Value<string>("email");
            if (!string.IsNullOrEmpty(email))
            {
                context.Identity.AddClaim(new Claim(
                     ClaimTypes.Email, email,
                    ClaimValueTypes.String, context.Options.ClaimsIssuer));
            }
        }

        private static async Task StravaTicketReceived(TicketReceivedContext context)
        {
            await Task.Delay(0);
        }
    }
}
