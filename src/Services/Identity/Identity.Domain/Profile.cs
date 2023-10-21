using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public class Profile
    {
        public Name? name { get; set; } = new Name();
        public string? nit { get; set; }
        public Image? image { get; set; } = new Image();
        public Address? address { get; set; } = new Address();
    }
}
