using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Behaviour;
using Order.Domain;
using Order.Utils;
using Serilog;
using System.Net;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Order.Api.Controllers
{
    [Route("[controller]/api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;

        public OrderController()
        {
            _logger = Log.ForContext<OrderController>();
        }

        [Authorize]
        [HttpPost]
        public Response InsertOrder([FromBody] dynamic data)
        {
            Domain.Order orden = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Order>(data.ToString());
            var identity = (ClaimsIdentity)User.Identity;
            var username = identity.Claims
                        .Where(c => c.Type == "username")
                        .Select(c => c.Value).FirstOrDefault();
            _logger.Information("Order.Api.order -- insert 1(s) order to orders collection...");
            OrderBehaviour pivot = new OrderBehaviour();
            return pivot.Insert(orden, username);
        }

        [HttpGet]
        public Response GetOrders()
        {
            _logger.Information("Order.Api.order -- get multiple orders from orders collection...");
            var identity = (ClaimsIdentity)User.Identity;
            var username = identity.Claims
                        .Where(c => c.Type == "username")
                        .Select(c => c.Value).FirstOrDefault();
            OrderBehaviour pivot = new OrderBehaviour();
            return pivot.get(username);
        }

        [HttpDelete]
        [Route("{order}")]
        public Response CancelOrder(string order)
        {
            _logger.Information("Order.Api.order -- delete 1(s) orders from orders collection...");
            var identity = (ClaimsIdentity)User.Identity;
            var username = identity.Claims
                        .Where(c => c.Type == "username")
                        .Select(c => c.Value).FirstOrDefault();
            OrderBehaviour pivot = new OrderBehaviour();
            return pivot.cancel(order, username);
        }
    }
}
