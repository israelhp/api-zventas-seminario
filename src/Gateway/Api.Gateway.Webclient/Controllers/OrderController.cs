using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http.Headers;
using System.Text;
using static Api.Gateway.Models.Order.DTOs.OrderDto;

namespace Api.Gateway.Webclient.Controllers
{
    [Route("orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        string url = "";
        public OrderController()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            url = configuration["MicroservicesUrls:OrderApiUrl"];
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] dynamic orden)
        { 
            try
            {
                var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var content = new StringContent(orden.ToString(), Encoding.UTF8, "application/json");
                var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
                var request = await _httpClient.PostAsync($"{url}", content);
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
        public async Task<IActionResult> GetAsync()
        { 
            try
            {
                var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var _httpClient = new HttpClient();
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

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
                var request = await _httpClient.DeleteAsync($"{url}/{id}");
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
