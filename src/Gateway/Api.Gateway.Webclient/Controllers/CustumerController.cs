using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using static Api.Gateway.Models.Custumer.DTOs.CustumerDto;

namespace Api.Gateway.Webclient.Controllers
{
    [Route("Custumer")]
    public class CustumerController : Controller
    {
        string url = "";
        public CustumerController()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            url = configuration["MicroservicesUrls:CustumerApiUrl"];
        }
        [HttpPost]
        public async Task<HttpResponseMessage> InsertUser([FromBody] User usuario)
        {
            var content = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
            var _httpClient = new HttpClient();
            var request = await _httpClient.PostAsync(url, content);
            return request;
        }
        [HttpPost]
        [Route("ResetPasswordRequest")]
        public async Task<HttpResponseMessage> PasswordResetRequest([FromBody] User usuario)
        {
            var content = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
            var _httpClient = new HttpClient();
            var request = await _httpClient.PostAsync($"{url}/ResetPasswordRequest", content);
            return request;
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<HttpResponseMessage> PasswordReset([FromBody] User usuario)
        {
            var content = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
            var _httpClient = new HttpClient();
            var request = await _httpClient.PostAsync($"{url}/ResetPassword", content);
            return request;
        }
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<HttpResponseMessage> ChangePassword([FromBody] dynamic passwords)
        {
            var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var content = new StringContent(JsonConvert.SerializeObject(passwords), Encoding.UTF8, "application/json");
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
            var request = await _httpClient.PostAsync($"{url}/ChangePassword", content);
            return request;
        }
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteUser()
        {
            var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
            var request = await _httpClient.DeleteAsync($"{url}");
            return request;
        }
        [HttpGet]
        public async Task<HttpResponseMessage> InfoUser()
        {
            var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
            var request = await _httpClient.GetAsync($"{url}");
            return request;
        }
        [HttpPut]
        [Route("profile")]
        public async Task<HttpResponseMessage> UpdateProfile([FromBody] dynamic profile)
        {
            var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var content = new StringContent(JsonConvert.SerializeObject(profile), Encoding.UTF8, "application/json");
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
            var request = await _httpClient.PutAsync($"{url}/profile", content);
            return request;
        }
    }
}
