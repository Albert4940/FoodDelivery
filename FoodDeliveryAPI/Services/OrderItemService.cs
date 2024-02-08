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
                item.Id = 0;
                item.OrderId = OrderId;
            }

            _context.AddRange(OrderItems);
            _context.SaveChangesAsync();
        }

        public static async Task<List<OrderItem>> GetAll()
        {
            return await _context.order_items.ToListAsync();
        }


        public static async Task<List<OrderItem>> GetByOrderID(long Id) => await _context.order_items.Where(o => o.OrderId == Id).ToListAsync();
    }
}
