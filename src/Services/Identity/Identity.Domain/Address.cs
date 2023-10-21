using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public class Address
    {
        public string? address { get; set; }
        public string? complement { get; set; }
        public string? country { get; set; }
        public string? county { get; set; }
        public string? city { get; set; }
        public string? postalCode { get; set; }
    }
}
