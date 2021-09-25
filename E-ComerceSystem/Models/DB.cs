using ITIMVCProjectV1.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace E_ComerceSystem.Models
{
    public partial class DB : IdentityDbContext<ApplicationUser>
    {
        public DB()
            : base("name=DB", throwIfV1Schema: false)
        {
        }


        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SubOrder> SubOrders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Feedback> Feadbacks { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<E_ComerceSystem.ModelView.CategoryModelView> CategoryModelViews { get; set; }
    }
}
