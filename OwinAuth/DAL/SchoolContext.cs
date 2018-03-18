using OwinAuth.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace OwinAuth.DAL
{
    public class SchoolContext : DbContext
    {
        public SchoolContext() : base("SchoolContext")
        {
        }

        public DbSet<Annoucement> Annoucements { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<AdminUser>()
                .HasMany<Role>(s => s.Roles)
                .WithMany(c => c.AdminUsers)
                .Map(cs =>
                {
                    cs.MapLeftKey("AdminUserId");
                    cs.MapRightKey("RoleId");
                    cs.ToTable("AdminUserRole");
                });
        }
    }
}