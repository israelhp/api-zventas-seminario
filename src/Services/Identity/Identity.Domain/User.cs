using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public class User
    {
        public ObjectId? _id { get; set; }
        public string? ID { get; set; }
        public required string email { get; set; }
        public required string? username { get; set; }
        public required string? password { get; set; }
        public required string? role { get; set; } = "2";
        public string? resetCode { get; set; }
        public Profile? profile { get; set; } = new Profile();
    }
}
