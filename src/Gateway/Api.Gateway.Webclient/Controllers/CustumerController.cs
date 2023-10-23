using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using static Api.Gateway.Models.Common.DTOs.CommonDto;
using static Api.Gateway.Models.Custumer.DTOs.CustumerDto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api.Gateway.Webclient.Controllers
{
    [Route("Custumer")]
    [ApiController]
    public class CustumerController : ControllerBase
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
        public async Task<IActionResult> InsertUser([FromBody] dynamic datos)
        {
            try
            {
                var content = new StringContent(datos.ToString(), Encoding.UTF8, "application/json");
                var _httpClient = new HttpClient();
                var request = await _httpClient.PostAsync(url, content);
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
        [HttpPost]
        [Route("ResetPasswordRequest")]
        public async Task<IActionResult> PasswordResetRequest([FromBody] dynamic usuario)
        {
            try
            {
                var content = new StringContent(usuario.ToString(), Encoding.UTF8, "application/json");
                var _httpClient = new HttpClient();
                var request = await _httpClient.PostAsync($"{url}/ResetPasswordRequest", content);
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

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> PasswordReset([FromBody] dynamic usuario)
        {
            try
            {
                var content = new StringContent(usuario.ToString(), Encoding.UTF8, "application/json");
                var _httpClient = new HttpClient();
                var request = await _httpClient.PostAsync($"{url}/ResetPassword", content);
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
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] dynamic passwords)
        {
            try
            {
                var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var content = new StringContent(passwords.ToString(), Encoding.UTF8, "application/json");
                var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
                var request = await _httpClient.PostAsync($"{url}/ChangePassword", content);
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
        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
                var request = await _httpClient.DeleteAsync($"{url}");
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
        [HttpGet]
        public async Task<IActionResult> InfoUser()
        {
            try
            {
                var _httpClient = new HttpClient();
                var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
                var request = await _httpClient.GetAsync($"{url}");
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
        [HttpPut]
        [Route("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] dynamic profile)
        {
            try
            {
                var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var content = new StringContent(profile.ToString(), Encoding.UTF8, "application/json");
                var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
                var request = await _httpClient.PutAsync($"{url}/profile", content);
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
