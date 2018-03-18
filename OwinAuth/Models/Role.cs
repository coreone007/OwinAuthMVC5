using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwinAuth.Models
{
    public class Role
    {
        public Role()
        {
            this.AdminUsers = new HashSet<AdminUser>();
        }
        public Guid RoleId { get; set; }
        public string Description { get; set; }
        public virtual ICollection<AdminUser> AdminUsers { get; set; }
    }
}