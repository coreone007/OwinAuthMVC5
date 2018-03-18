using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwinAuth.Models
{
    public class AdminUser
    {
        public AdminUser()
        {
            this.Roles = new HashSet<Role>();
        }
        public Guid AdminUserId { get; set; }
        public string UserGroup { get; set; }
        public bool IsGroup { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}