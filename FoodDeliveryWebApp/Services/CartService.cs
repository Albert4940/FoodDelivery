using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using Microsoft.EntityFrameworkCore;

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

        public static async Task Add(Cart cart)
        {
            _context.Add(cart);
            await _context.SaveChangesAsync();
        }

        public static async Task Update(Cart cart)
        {
            _context.Entry(cart).State = EntityState.Modified;
            _context.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}
