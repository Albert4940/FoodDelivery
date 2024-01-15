using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryAPI.Data
{
    public static class SeedData
    {
        public static void  InitializeAsync(IServiceProvider serviceProvider)
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

                /*var user = context.users.First(u => u.UserName == "albert_admin");

                context.categories.AddRange(
                        new Category
                        {
                            Id= 0,
                          Title =  "Fruits",
                          IconUrl = "string",
                          UserId = user.Id
                        }
                    );
                context.SaveChanges();*/
            }
        }
    }
}
