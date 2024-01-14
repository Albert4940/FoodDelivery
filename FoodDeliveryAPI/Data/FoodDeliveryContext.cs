using FoodDeliveryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryAPI.Data
{
    public class FoodDeliveryContext : DbContext
    {
        public FoodDeliveryContext(DbContextOptions<FoodDeliveryContext> options) : base(options) { }

        public DbSet<User> users { get; set; } = null;
        public DbSet<Category>  categories{ get; set; } = null;
        public DbSet<Food> foods { get; set; } = null;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Title)
                .IsUnique();
        }
    }
}
