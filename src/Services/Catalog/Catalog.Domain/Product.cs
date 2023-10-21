using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Catalog.Domain
{
    public class Product
    {
        public ObjectId _id { get; set; }
        public string? code { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public double? quantity { get; set; }
        public double? discount { get; set; }
        public Category? category { get; set; } = new Category();
        public Brand? brand { get; set; } = new Brand();
        public double? price { get; set; }
        public double? tax { get; set; }
        public Image? image { get; set; } = new Image();
        public string? qr_id { get; set; }
        public List<Image>? carrete { get; set; } = new List<Image>();
    }
}
