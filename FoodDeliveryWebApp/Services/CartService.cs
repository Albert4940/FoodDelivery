using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace FoodDeliveryWebApp.Services
{
    public class CartService : BaseService
    {

        public CartService(FoodDeliveryWebAppDbContext context) : base(context) { }

        public async Task<Order> Get(string UserId)
            {
                if (string.IsNullOrWhiteSpace(UserId) || UserId == "")
                    throw new ArgumentOutOfRangeException(nameof(UserId), "The UserId cannot be null or empty.");

                var carts = await base.Get<Order>();
               
                return carts.FirstOrDefault(c => c.UserId == UserId);
        }

        public async Task<Order> Add(string UserId)
        {
            var cart = await Get(UserId);

            if (cart is null)
            {
                await base.Add<Order>(new Order() { UserId = UserId, ItemsPrice = 0, TaxPrice = 0, DeliveryFee = 0, TotalPrice = 0 });
                return await Get(UserId);
            }

            return cart;
        }
        /*public static async Task Add(Order cart)
        {
            _context.Add(cart);
            await _context.SaveChangesAsync();
        }

        public static async Task Update(Order cart)
        {
            _context.Entry(cart).State = EntityState.Modified;
            _context.Update(cart);
            await _context.SaveChangesAsync();
        }*/
    }
}
