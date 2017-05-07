using StravaReporter.Models.Strava;
using System.Net.Http;
using System.Threading.Tasks;

namespace StravaReporter.Services
{

    public class StravaConnector : IStravaConnector
    {
        private readonly IAccessTokenProvider _tokenProvider;
        public StravaConnector(IAccessTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public async Task<string> GetDataAsync(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = Constants.ApiBaseUrl;
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _tokenProvider.Token);
                var json = await client.GetStringAsync(url);
                return json;
            }
        }
    }

}