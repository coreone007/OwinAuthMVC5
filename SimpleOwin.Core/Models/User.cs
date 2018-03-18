using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace SimpleOwin.Core.Models
{
    public class User : IUser<long>
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public List<string> Roles { get; set; }
    }
}