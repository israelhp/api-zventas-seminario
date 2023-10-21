using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Api.Gateway.Webclient.Controllers
{
    public class IdentityController : Controller
    {
        string url = "";
        public IdentityController()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            url = configuration["MicroservicesUrls:IdentityApiUrl"];
        }
        [HttpPost]
        public async Task<HttpResponseMessage> token([FromForm] string username, [FromForm] string password, [FromForm] string grant_type)
        {
            var requestData = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
                { "grant_type", grant_type }
            };
            var formContent = new FormUrlEncodedContent(requestData);
            var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
            var request = await _httpClient.PostAsync($"{url}/token", formContent);

            return request;
        }
    }
}
