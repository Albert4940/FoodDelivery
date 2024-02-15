using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;

namespace FoodDeliveryWebApp.Services
{
    public static class BaseService
    {
        private static FoodDeliveryWebAppDbContext _context;

        public static void InintializeContextDb(FoodDeliveryWebAppDbContext context)
        {
            _context = context;

        }

        public static T Get<T>() where T : class => _context.Set<T>().FirstOrDefault();
    }
}
