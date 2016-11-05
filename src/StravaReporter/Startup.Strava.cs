using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace StravaReporter
{
    public partial class Startup
    {
        private OAuthOptions StravaOptions
        {
            get
            {
                return new OAuthOptions
                {
                    // We need to specify an Authentication Scheme
                    AuthenticationScheme = "Strava",

                    ClientId = Configuration["Authentication:Strava:ClientId"],
                    ClientSecret = Configuration["Authentication:Strava:ClientSecret"],

                    CallbackPath = new PathString("/signin-strava"),

                    AuthorizationEndpoint = "https://www.strava.com/oauth/authorize",
                    TokenEndpoint = "https://www.strava.com/oauth/token",
                    UserInformationEndpoint = "https://www.strava.com/api/v3/athlete",

                    Scope = { "view_private" },

                    Events = new OAuthEvents
                    {
                        // The OnCreatingTicket event is called after the user has been authenticated and the OAuth middleware has
                        // created an auth ticket. We need to manually call the UserInformationEndpoint to retrieve the user's information,
                        // parse the resulting JSON to extract the relevant information, and add the correct claims.
                        OnCreatingTicket = async context => { await CreatingStravaAuthTicket(context); }
                    }
                };
            }
        }

        /// <summary>
        /// Creates tickets and add claims. Claims are "fetched" via signin later.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task CreatingStravaAuthTicket(OAuthCreatingTicketContext context)
        {
            // Retrieve user info
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            request.Headers.Add("x-li-format", "json"); // Tell LinkedIn we want the result in JSON, otherwise it will return XML

            var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            // Extract the user info object
            var user = JObject.Parse(await response.Content.ReadAsStringAsync());

            // Add token:
            context.Identity.AddClaim(new Claim("urn:strava:accesstoken", context.AccessToken, ClaimValueTypes.String, context.Options.ClaimsIssuer));

            // Add the Name Identifier claim
            var userId = user.Value<string>("id");
            if (!string.IsNullOrEmpty(userId))
            {
                context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String, context.Options.ClaimsIssuer));
            }

            // Add the Name claim
            var firstName = user.Value<string>("firstname");
            if (!string.IsNullOrEmpty(firstName))
            {
                context.Identity.AddClaim(new Claim(ClaimTypes.Name, firstName, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                context.Identity.AddClaim(new Claim(ClaimTypes.GivenName, firstName, ClaimValueTypes.String, context.Options.ClaimsIssuer));
            }

            var lastName = user.Value<string>("lastname");
            if (!string.IsNullOrEmpty(lastName))
            {
                context.Identity.AddClaim(new Claim(ClaimTypes.Surname, lastName, ClaimValueTypes.String, context.Options.ClaimsIssuer));
            }

            // Add the email address claim
            var email = user.Value<string>("email");
            if (!string.IsNullOrEmpty(email))
            {
                context.Identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));
            }

            // Add the Profile Picture claim
            var pictureUrl = user.Value<string>("profile");
            if (!string.IsNullOrEmpty(pictureUrl))
            {
                context.Identity.AddClaim(new Claim("profile-picture", pictureUrl, ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));
            }
        }
    }
}
