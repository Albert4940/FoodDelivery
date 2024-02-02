using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryAPI.Services
{
    public class OrderItemService
    {
        private static FoodDeliveryContext _context;
        public static void InitializeContext(FoodDeliveryContext context)
        {
            _context = context;
        }

        public static async Task AddRange(List<OrderItem> OrderItems, long OrderId)
        {
            if (OrderId == 0)
                throw new Exception("OrderId null");

            foreach (var item in OrderItems)
            {
                item.OrderId = OrderId;
            }
            _context.AddRange(OrderItems);
            _context.SaveChangesAsync();
        }

        public static async Task<List<OrderItem>> GetAll()
        {
            return await _context.order_items.ToListAsync();
        }
    }
}
