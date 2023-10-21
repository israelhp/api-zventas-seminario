using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Gateway.Models.Custumer.DTOs
{
    public class CustumerDto
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
        public class Image
        {
            public int? id { get; set; }
            public string? name { get; set; }
            public string? extension { get; set; }
            public string? url { get; set; }
            public string? imageType { get; set; }
        }
        public class Mail
        {
            public ObjectId? _id { get; set; }
            public string? email { get; set; }
            public string? password { get; set; }
        }
        public class Name
        {
            public string? firstName { get; set; }
            public string? middleName { get; set; }
            public string? lastName { get; set; }
            public string? lastName2 { get; set; }
        }
        public class Profile
        {
            public Name? name { get; set; } = new Name();
            public string? nit { get; set; }
            public Image? image { get; set; } = new Image();
            public Address? address { get; set; } = new Address();
        }
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
}
