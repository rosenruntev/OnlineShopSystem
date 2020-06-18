using Microsoft.EntityFrameworkCore;
using OSS.Models.Entities;

namespace OSS.Data
{
    public class ShopSystemDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Server=DESKTOP-SOTOIG4\SQLEXPRESS;" +
                              @"DataBase=OnlineShopSystem;" +
                              @"Integrated Security=true;");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
