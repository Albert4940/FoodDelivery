using FoodDeliveryAPI.Models;
using FoodDeliveryWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryAPI.Data
{
    public class FoodDeliveryContext : DbContext
    {
        public FoodDeliveryContext(DbContextOptions<FoodDeliveryContext> options) : base(options) { }

        public DbSet<User> users { get; set; } = null;
        public DbSet<Category>  categories{ get; set; } = null;
        public DbSet<Food> foods { get; set; } = null;

        public DbSet<Order> order { get; set; } = null;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Title)
                .IsUnique();
            modelBuilder.Entity<Category>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Food>()
            .HasIndex(f => f.Title)
            .IsUnique();

            modelBuilder.Entity<Food>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(f => f.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Food>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
