using Catalog.Behaviour;
using Catalog.Domain;
using Catalog.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Catalog.Api.Controllers
{
    [Route("[controller]/api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;

        public ProductController()
        {
            _logger = Log.ForContext<ProductController>();
        }

        [HttpGet]
        [Route("{code}")]
        public Response GetProductByCode(string code)
        {
            _logger.Information("Catalog.Api.products -- get 1(s) product from products collection...");
            ProductBehaviour pivot = new ProductBehaviour();
            return pivot.GetByCode(code);
        }

        [HttpPost]
        [Route("list")]
        public Response GetProducts(dynamic datos)
        {
            var filtros = JsonConvert.DeserializeObject<ProductFilters>(datos.ToString());
            _logger.Information("Catalog.Api.products -- get from list with filters from products collection...");
            ProductBehaviour pivot = new ProductBehaviour();
            return pivot.Get(filtros);
        }

        [HttpGet]
        [Route("categories")]
        public Response GetCategories()
        {
            _logger.Information("Catalog.Api.products -- get categories with no filters from products collection...");
            ProductBehaviour pivot = new ProductBehaviour();
            return pivot.Categories();
        }

        [Authorize(Roles = "1,admin")]
        [HttpPost]
        public Response InsertProduct(dynamic datos)
        {
            var producto = JsonConvert.DeserializeObject<Product>(datos.ToString());
            _logger.Information("Catalog.Api.products -- post insert 1(s) register into products collection...");
            ProductBehaviour pivot = new ProductBehaviour();
            return pivot.Insert(producto);
        }

        [Authorize(Roles = "1,admin")]
        [HttpDelete]
        [Route("{code}")]
        public Response DeleteProducts(string code)
        {
            _logger.Information("Catalog.Api.products -- delete deleted 1(s) products into products collection...");
            ProductBehaviour pivot = new ProductBehaviour();
            return pivot.Delete(code);
        }

        [Authorize(Roles = "1,admin")]
        [HttpPut]
        [Route("{code}")]
        public Response UpdateProduct(dynamic producto, string code)
        {
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject(producto.ToString());
            _logger.Information("Catalog.Api.products -- put updated 1(s) products into products collection...");
            ProductBehaviour pivot = new ProductBehaviour();
            return pivot.Update(json, code);
        }
    }
}
