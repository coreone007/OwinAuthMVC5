using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwinAuth.Models
{
    public class ResultObject
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}