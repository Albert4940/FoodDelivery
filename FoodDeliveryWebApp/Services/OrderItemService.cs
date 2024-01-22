using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryWebApp.Services
{
    public static class OrderItemService
    {
        private static FoodDeliveryWebAppDbContext _context;
        public static void InintializeContextDb(FoodDeliveryWebAppDbContext context)
        {
            _context = context;
        }

        public static OrderItem GetByID(long Id) => _context.OrderItems.FirstOrDefault(o => o.Id == Id);

        public static async Task Update(OrderItem orderItem)
        {
            _context.Entry(orderItem).State = EntityState.Modified;
            _context.Update(orderItem);
            await _context.SaveChangesAsync();
        }
    }
}
