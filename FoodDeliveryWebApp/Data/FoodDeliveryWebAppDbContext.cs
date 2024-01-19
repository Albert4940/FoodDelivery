using FoodDeliveryWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryWebApp.Data
{
    public class FoodDeliveryWebAppDbContext : DbContext
    {
        public FoodDeliveryWebAppDbContext(DbContextOptions<FoodDeliveryWebAppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
             .HasOne<Cart>()
             .WithMany()
             .HasForeignKey(o => o.CartId)
             .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
