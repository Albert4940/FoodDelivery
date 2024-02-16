using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryWebApp.Services
{
    public static class BaseService
    {
        private static FoodDeliveryWebAppDbContext _context;

        public static void InintializeContextDb(FoodDeliveryWebAppDbContext context)
        {
            _context = context;

        }

        public static T Get<T>(long Id = 0) where T : IEntity => Id == 0 
            ? _context.Set<T>().AsNoTracking().FirstOrDefault() 
            : _context.Set<T>().AsNoTracking().FirstOrDefault(t => t.Id == Id);

    }
}
