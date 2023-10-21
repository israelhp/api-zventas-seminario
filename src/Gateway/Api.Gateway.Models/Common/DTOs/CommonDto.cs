using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Api.Gateway.Models.Common.DTOs
{
    public class CommonDto
    {
        public class Response
        {
            public HttpStatusCode? code { get; set; }
            public string? message { get; set; }
            public dynamic? data { get; set; }
        }
    }
}
