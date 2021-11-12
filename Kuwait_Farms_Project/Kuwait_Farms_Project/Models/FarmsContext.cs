using System;
using System.Data.Entity;
using System.Linq;

namespace Kuwait_Farms_Project.Models
{
    public class FarmsContext : DbContext
    {
       
        public FarmsContext()
            : base("name=FarmsContext")
        {
        }

       

         public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<ContactUs> GetContactUs { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Sales> GetSales { get; set; }
    }


}