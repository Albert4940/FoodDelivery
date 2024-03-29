﻿using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryAPI.Services
{
    public static class CategoryService
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
        public static async Task<List<Category>> GetAll() => await _context.categories.ToListAsync();
        public static async Task<Category> Get(Category category) => await _context.categories.AsNoTracking().FirstOrDefaultAsync(c => c.Title.Equals(category.Title));
        public static async Task<Category>? GetByID(long? Id) => await _context.categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == Id);

        public static async Task Add(Category category)
        {
            //That generate by database
            //fount way to pass item without indicat id because it generate by db
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

        public static async Task Delete(Category cat)
        {
            _context.categories.Remove(cat);
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
