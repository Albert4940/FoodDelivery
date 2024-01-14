using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryAPI.Services
{
    public static class FoodService
    {
        private static FoodDeliveryContext _context;
        // Method to initialize the context
        public static void InitializeContext(FoodDeliveryContext context)
        {
            _context = context;
        }

        public static async Task<List<Food>> GetAll() => await _context.foods.ToListAsync();
        public static async Task Add(Food food)
        {
            //That generate by database    
            food.Id = 0;
            _context.Add(food);
            await _context.SaveChangesAsync();
        }
    }
}
