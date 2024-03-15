using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
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
            order.Id = 0;

            _context.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public static async Task<Order> GetByID(long Id) => await _context.orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == Id);
        public static async Task<List<Order>> GetAll()
        {
            return await _context.orders.ToListAsync();
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

        public static async Task Delete(Order order)
        {
            _context.orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
