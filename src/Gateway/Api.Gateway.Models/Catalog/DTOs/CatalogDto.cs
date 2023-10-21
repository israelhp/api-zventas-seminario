using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Api.Gateway.Models.Catalog.DTOs
{
    public class CatalogDto
    {
        public class Brand
        {
            public int? id { get; set; }
            public string? name { get; set; }
        }
        public class Category
        {
            public int? id { get; set; }
            public string? name { get; set; }
        }
        public class Image
        {
            public int? id { get; set; }
            public string? name { get; set; }
            public string? extension { get; set; }
            public string? url { get; set; }
            public string? imageType { get; set; }
        }
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
        public class ProductFilters
        {
            public bool? filter { get; set; } = false;
            public string? brand_eq { get; set; }
            public string? category_eq { get; set; }
            public double? Price_gt { get; set; }
            public double? Price_lt { get; set; }
            public string? search { get; set; }
            public int? page { get; set; }
            public int? ItemsPerPage { get; set; }
        }
    }
}
