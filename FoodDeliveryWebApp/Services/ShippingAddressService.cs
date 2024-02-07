using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;

namespace FoodDeliveryWebApp.Services
{
    public class ShippingAddressService
    {
        private static FoodDeliveryWebAppDbContext _context;

        public static void InintializeContextDb(FoodDeliveryWebAppDbContext context)
        {
            _context = context;

        }

        public static ShippingAddress Get() => _context.ShippingAddresses.FirstOrDefault();

        public static async Task Add(ShippingAddress ShippingAddress)
        {
            ShippingAddress.UserId = "String";
            _context.Add(ShippingAddress);
            await _context.SaveChangesAsync();
        }
    }
}
