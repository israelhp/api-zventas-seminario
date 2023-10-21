using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain
{
    public class Payment
    {
        public string? type { get; set; }
        public double? amount { get; set; }
        public CreditCard? card { get; set; } = new CreditCard();
    }
}
