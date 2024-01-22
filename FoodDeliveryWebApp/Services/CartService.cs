using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;

namespace FoodDeliveryWebApp.Services
{
    public static class CartService
    {
        private static  FoodDeliveryWebAppDbContext _context;

        public static void InintializeContextDb( FoodDeliveryWebAppDbContext context)
        {
            _context = context;
        }

        public static Cart Get() => _context.Carts.FirstOrDefault();
    }
}
