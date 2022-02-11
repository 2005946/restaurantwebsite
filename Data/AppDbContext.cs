using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantWebsite.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantWebsite.wwwroot.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }
        public DbSet<menu> tblMenu { get; set; }

        public DbSet<CheckoutCustomer> CheckoutCustomers { get; set; }

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<BasketItem> BasketItems { get; set; }

        public DbSet<OrderHistory> OrderHistories { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<UserOrder> UserOrders { get; set; }

        [NotMapped]
        public DbSet<CheckoutItem> CheckoutItems { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BasketItem>().HasKey(t => new { t.StockID, t.BasketID });
            builder.Entity<OrderItem>().HasKey(t => new { t.StockID, t.OrderNo });
        }
    }
}
