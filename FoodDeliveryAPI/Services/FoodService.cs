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
        public static async Task<Food> GetByID(long id) => await _context.foods.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
        public static async Task<Food> Get(Food food) => await _context.foods.AsNoTracking().FirstOrDefaultAsync(f => f.Title.Equals(food.Title));
        public static async Task Add(Food food)
        {
            //That generate by database    
            food.Id = 0;
            _context.Add(food);
            await _context.SaveChangesAsync();
        }

        public static async Task Update(Food food)
        {
            //That generate by database 
            _context.Entry(food).State = EntityState.Modified;
            _context.Update(food);
            await _context.SaveChangesAsync();
        }

        public static async Task UpdateCountInStock(List<OrderItem> OrderItems)
        {
            long[] FoodIds = new long[OrderItems.Count];

            for(int i = 0; i < OrderItems.Count; i++)
            {
                FoodIds[i] = OrderItems[i].ProductId;
            }

            
            var records = _context.foods.Where(f => FoodIds.Contains(f.Id)).ToList();
            int countRecords = records == null ? 0 : records.Count;

            //return the food id is missing
            if (countRecords != FoodIds.Length)
                throw new Exception("Some food missing");
           
            for (int i = 0; i < OrderItems.Count; i++)
            {
                var qty = records[i].CountInStock - OrderItems[i].Qty;
                records[i].CountInStock = qty;
            }
            await _context.SaveChangesAsync();
        }

        public static async Task Delete(Food food)
        {
            _context.foods.Remove(food);
            await _context.SaveChangesAsync();

        }

        public static async Task<bool> ComapreQty(OrderItem foodOrder)
        {
            var food = await GetByID(foodOrder.ProductId);

            if (food != null)
            {
                if (food.CountInStock >= foodOrder.Qty)
                {
                    return true;
                }
                else throw new Exception($"Insufficient Stock : {food.Title} - Count In Stock = {food.CountInStock}");
            }
            else throw new Exception($"Food Order Not Found ");
            
        }
        public static async Task<bool> CheckIfFoodExists(Food food) => await Get(food) != null;

        public static async Task<bool> CheckIfFoodExistsForUpdate(Food food)
        {
            var foodResult = await Get(food);
            if (foodResult != null && food.Id != foodResult.Id)
                return true;
            return false;
        }
    }
}
