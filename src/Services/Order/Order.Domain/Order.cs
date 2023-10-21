using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Order.Domain
{
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
}
