using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using static Api.Gateway.Models.Catalog.DTOs.CatalogDto;

namespace Api.Gateway.Webclient.Controllers
{
    [Route("Catalog")]
    [ApiController]
    public class CatalogController : ControllerBase
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
        public async Task<IActionResult> GetProductByCode(string code)
        {
            try
            {
                var _httpClient = new HttpClient();
                var request = await _httpClient.GetAsync($"{url}/{code}");
                var responseContent = await request.Content.ReadAsStringAsync();
                if (request.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                if (request.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return Forbid();
                }
                return Content(responseContent, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest("Error interno: " + ex.Message);
            }
        }
        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> GetProducts([FromBody] dynamic filtros)
        {
            try
            {
                var content = new StringContent(filtros.ToString(), Encoding.UTF8, "application/json");
                var _httpClient = new HttpClient();
                var request = await _httpClient.PostAsync($"{url}/list", content);
                var responseContent = await request.Content.ReadAsStringAsync();
                if (request.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                if (request.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return Forbid();
                }
                return Content(responseContent, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest("Error interno: " + ex.Message);
            }
        }
        [HttpGet]
        [Route("categories")]
        public async Task<IActionResult> GetCategories()
        {  
            try
            {
                var _httpClient = new HttpClient();
                var request = await _httpClient.GetAsync($"{url}/categories");
                var responseContent = await request.Content.ReadAsStringAsync();
                if (request.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                if (request.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return Forbid();
                }
                return Content(responseContent, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest("Error interno: " + ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertProduct([FromBody] dynamic producto)
        {
            try
            {
                var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var content = new StringContent(producto.ToString(), Encoding.UTF8, "application/json");
                var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
                var request = await _httpClient.PostAsync($"{url}", content);
                var responseContent = await request.Content.ReadAsStringAsync();
                if (request.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                if (request.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return Forbid();
                }
                return Content(responseContent, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest("Error interno: " + ex.Message);
            }
        }
        [HttpDelete]
        [Route("{code}")]
        public async Task<IActionResult> DeleteProducts(string code)
        {
            try
            {
                var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
                var request = await _httpClient.DeleteAsync($"{url}/{code}");
                var responseContent = await request.Content.ReadAsStringAsync();
                if (request.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                if (request.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return Forbid();
                }
                return Content(responseContent, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest("Error interno: " + ex.Message);
            }
        }
        [HttpPut]
        [Route("{code}")]
        public async Task<IActionResult> UpdateProduct(dynamic producto, string code)
        {
            try
            {
                var _bearer_token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var content = new StringContent(JsonConvert.SerializeObject(producto), Encoding.UTF8, "application/json");
                var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer_token);
                var request = await _httpClient.PutAsync($"{url}/{code}", content);
                var responseContent = await request.Content.ReadAsStringAsync();
                if (request.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                if (request.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return Forbid();
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
