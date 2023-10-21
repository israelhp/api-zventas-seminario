using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using static Api.Gateway.Models.Catalog.DTOs.CatalogDto;

namespace Api.Gateway.Webclient.Controllers
{

    [Route("Catalog")]
    public class CatalogController : Controller
    {
        string url = "";
        public CatalogController()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            url = configuration["MicroservicesUrls:CatalogApiUrl"];
        }
        [HttpGet]
        [Route("{code}")]
        public async Task<HttpResponseMessage> GetProductByCode(string code)
        {
            var _httpClient = new HttpClient();
            var request = await _httpClient.GetAsync($"{url}/{code}");
            return request;
        }
        [HttpPost]
        [Route("list")]
        public async Task<HttpResponseMessage> GetProducts([FromBody] ProductFilters filtros)
        {
            var content = new StringContent(JsonConvert.SerializeObject(filtros), Encoding.UTF8, "application/json");
            var _httpClient = new HttpClient();
            var request = await _httpClient.PostAsync($"{url}/list", content);
            return request;
        }
        [HttpGet]
        [Route("categories")]
        public async Task<HttpResponseMessage> GetCategories()
        {
            var _httpClient = new HttpClient();
            var request = await _httpClient.GetAsync($"{url}/categories");
            return request;
        }
        [HttpPost]
        public async Task<HttpResponseMessage> InsertProduct([FromBody] Product producto)
        {
            var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var content = new StringContent(JsonConvert.SerializeObject(producto), Encoding.UTF8, "application/json");
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
            var request = await _httpClient.PostAsync($"{url}", content);
            return request;
        }
        [HttpDelete]
        [Route("{code}")]
        public async Task<HttpResponseMessage> DeleteProducts(string code)
        {
            var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
            var request = await _httpClient.DeleteAsync($"{url}/{code}");
            return request;
        }
        [HttpPut]
        [Route("{code}")]
        public async Task<HttpResponseMessage> UpdateProduct(dynamic producto, string code)
        {
            var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var content = new StringContent(JsonConvert.SerializeObject(producto), Encoding.UTF8, "application/json");
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
            var request = await _httpClient.PutAsync($"{url}/{code}", content);
            return request;
        }
    }
}
