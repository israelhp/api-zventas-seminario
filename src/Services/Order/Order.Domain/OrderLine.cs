using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain
{
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
}
