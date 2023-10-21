using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public class Image
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? extension { get; set; }
        public string? url { get; set; }
        public string? imageType { get; set; }
    }
}
