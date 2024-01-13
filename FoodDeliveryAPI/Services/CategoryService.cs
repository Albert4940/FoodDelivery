using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryAPI.Services
{
    public class CategoryService
    {
        //Generics class Services for entity
        private static FoodDeliveryContext _context;
        // Method to initialize the context
        public static void InitializeContext(FoodDeliveryContext context)
        {
            _context = context;
        }


        /*public static User? Get(User user) => Users.FirstOrDefault(x => x.UserName.Equals(user.UserName, StringComparison.CurrentCultureIgnoreCase)
            && x.Password == user.Password);*/
        public static async Task<Category> Get(Category category) => await _context.categories.FirstOrDefaultAsync(c => c.Title.Equals(category.Title));
        public static async Task<Category>? GetByID(int? Id) => await _context.categories.FirstOrDefaultAsync(c => c.Id == Id);

        public static async Task Add(Category category)
        {
            //That generate by database    
            category.Id = 0;
            _context.Add(category);
            await _context.SaveChangesAsync();
        }

        public static async Task Update(Category cat)
        {
            //That generate by database 
            _context.Entry(cat).State = EntityState.Modified;
            _context.Update(cat);
            await _context.SaveChangesAsync();
        }

        //Factorize it inside the intreface
        public static async Task<bool> CheckIfCategoryExists(Category category) => await CategoryService.Get(category) != null;
        public static async Task<bool> CheckIfCategoryExistsForUpdate(Category category) {
            var catResult = await CategoryService.Get(category);
            if (catResult != null && category.Id != catResult.Id)
                return true;
            return false;
            }
    }
}
