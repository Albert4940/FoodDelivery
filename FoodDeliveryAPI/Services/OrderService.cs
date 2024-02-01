using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using FoodDeliveryWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryAPI.Services
{
    public static class OrderService
    {
        private static FoodDeliveryContext _context;
        public static void InitializeContext(FoodDeliveryContext context)
        {
            _context = context;
        }
        public static async Task<Order> Add(Order order)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public static async Task<decimal> GetTotalPrice(List<OrderItem> OrderItems)
        {
            decimal total = 0;

            foreach (var item in OrderItems)
            {
                if (await FoodService.ComapreQty(item))
                    total += item.Price * item.Qty;
            }

            return total;
        }
    }
}
