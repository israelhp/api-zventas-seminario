using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain
{
    public class AuthInfo
    {
        public bool? status { get; set; }
        public string? username { get; set; }
        public string? role { get; set; }
    }
}
