using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Data;

namespace FoodDeliveryWebApp.Services
{
    public class OrderItemService : BaseService
    {
        /*private static FoodDeliveryWebAppDbContext _context;
        public static void InintializeContextDb(FoodDeliveryWebAppDbContext context)
        {
            _context = context;
        }

        public static OrderItem GetByID(long Id) => _context.OrderItems.FirstOrDefault(o => o.Id == Id);

        public static async Task<List<OrderItem>> GetAll() => await _context.OrderItems.ToListAsync();

        public static async Task Add(OrderItem orderItem)
        {
            _context.Add(orderItem);
            await _context.SaveChangesAsync();
        }
        public static async Task Update(OrderItem orderItem)
        {
            _context.Entry(orderItem).State = EntityState.Modified;
            _context.Update(orderItem);
            await _context.SaveChangesAsync();
        }

        public static async Task Remove(OrderItem orderItem)
        {
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
        }*/

        public OrderItemService(FoodDeliveryWebAppDbContext context) : base(context) { }
        public async Task AddOrderItem(Food food, long cartId, int qty)
        {

            //var OrderItem = _context.OrderItems.FirstOrDefault(o => o.ProductId == food.Id && o.CartId == cartId);
            var OrderItem = _context.OrderItems.AsNoTracking().FirstOrDefault(o => o.ProductId == food.Id && o.CartId == cartId);

            try
            {
                if (OrderItem is null)
                {
                    await base.Add<OrderItem>(new OrderItem()
                    {
                        Title = food.Title,
                        CartId = cartId,
                        Qty = qty,
                        ImageURL = food.ImageURL,
                        Price = food.Price,
                        ProductId = food.Id,
                        CountInStock = food.CountInStock
                    });
                }
                else
                {
                    OrderItem.Qty = qty;
                    OrderItem.CountInStock = food.CountInStock;
                    await base.Update<OrderItem>(OrderItem);
                }
            }
            catch (Exception ex)
            {                
                throw;
            }


        }
    }
}
