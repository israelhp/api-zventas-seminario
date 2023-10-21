using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Custumer.Domain
{
    public class Mail
    {
        public ObjectId? _id { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
    }
}
