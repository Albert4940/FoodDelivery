using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryWebApp.Services
{
    public class ShippingAddressService
    {
        private static FoodDeliveryWebAppDbContext _context;

        public static void InintializeContextDb(FoodDeliveryWebAppDbContext context)
        {
            _context = context;

        }

        public static ShippingAddress Get() => _context.ShippingAddresses.AsNoTracking().FirstOrDefault();

        public static async Task Add(ShippingAddress ShippingAddress)
        {
            ShippingAddress.UserId = "String";
            _context.Add(ShippingAddress);
            await _context.SaveChangesAsync();
        }

        public static async Task Update(ShippingAddress entity)
        {
            entity.UserId = "String";
            _context.Entry(entity).State = EntityState.Modified;
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
