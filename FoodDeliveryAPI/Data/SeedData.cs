using FoodDeliveryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryAPI.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FoodDeliveryContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<FoodDeliveryContext>>()))
            {
                // Look for any movies.
                if (context.users.Any())
                {
                    return;   // DB has been seeded
                }
                context.users.AddRange(
                    new User
                    {
                        UserName = "albert_admin", Password="1234"
            
                    },
                    new User
                    {
                         UserName = "dorce_admin", Password="1234"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
