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

        public static Order Get() => _context.Orders.FirstOrDefault();

        public static async Task Add(Order cart)
        {
            _context.Add(cart);
            await _context.SaveChangesAsync();
        }

        public static async Task Update(Order cart)
        {
            _context.Entry(cart).State = EntityState.Modified;
            _context.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}
