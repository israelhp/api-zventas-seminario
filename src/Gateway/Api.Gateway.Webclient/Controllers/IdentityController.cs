using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Api.Gateway.Webclient.Controllers
{
    [Route("token")]
    [ApiController]
    public class IdentityController : ControllerBase
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
        public async Task<IActionResult> token([FromForm] string username, [FromForm] string password, [FromForm] string grant_type)
        {
            try
            {
                var requestData = new Dictionary<string, string>
                {
                    { "username", username },
                    { "password", password },
                    { "grant_type", grant_type }
                };
                var formContent = new FormUrlEncodedContent(requestData);
                var _httpClient = new HttpClient();
                var request = await _httpClient.PostAsync($"{url}/token", formContent);
                var responseContent = await request.Content.ReadAsStringAsync();
                if (request.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                return Content(responseContent, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest("Error interno: " + ex.Message);
            }
        }
    }
}
