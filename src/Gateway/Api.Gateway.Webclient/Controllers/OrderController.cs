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
    public class OrderController : Controller
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
        public async Task<HttpResponseMessage> CreateAsync([FromBody] Order orden)
        {
            var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var content = new StringContent(JsonConvert.SerializeObject(orden), Encoding.UTF8, "application/json");
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
            var request = await _httpClient.PostAsync($"{url}", content);
            return request;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> CreateAsyncGet()
        {
            var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
            var request = await _httpClient.GetAsync($"{url}");
            return request;
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> CreateAsyncDelete(string id)
        {
            var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
            var request = await _httpClient.DeleteAsync($"{url}/{id}");
            return request;
        }
    }
}
