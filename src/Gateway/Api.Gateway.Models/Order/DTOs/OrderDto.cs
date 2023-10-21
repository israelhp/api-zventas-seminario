using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api.Gateway.Models.Order.DTOs
{
    public class OrderDto
    {
        public class CreditCard
        {
            public string? number { get; set; }
            public int? bin { get; set; }
        }
        public class Order
        {
            public ObjectId? _id { get; set; }
            public string? userId { get; set; }
            public DateTime? date { get; set; }
            public List<OrderLine>? lines { get; set; }
            public double? subtotal { get; set; }
            public double? discount { get; set; }
            public double? deliveryAmount { get; set; }
            public double? tax { get; set; }
            public double? grandTotal { get; set; }
            public string? status { get; set; }
            public Payment? payment { get; set; } = new Payment();
        }
        public class OrderLine
        {
            public int? lineNum { get; set; }
            public string? code { get; set; }
            public string? description { get; set; }
            public double? quantity { get; set; }
            public double? discount { get; set; }
            public double? price { get; set; }
            public bool? iva { get; set; }
            public double? tax { get; set; }
        }
        public class Payment
        {
            public string? type { get; set; }
            public double? amount { get; set; }
            public CreditCard? card { get; set; } = new CreditCard();
        }
        public class Response
        {
            public HttpStatusCode? code { get; set; }
            public string? message { get; set; }
            public dynamic? data { get; set; }
        }
    }
}
