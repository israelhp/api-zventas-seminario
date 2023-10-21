using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain
{
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
