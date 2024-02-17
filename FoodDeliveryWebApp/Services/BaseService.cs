using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryWebApp.Services
{
    public class BaseService
    {
        protected FoodDeliveryWebAppDbContext _context;

        public BaseService(FoodDeliveryWebAppDbContext context)
        {
            _context = context;

        }

        public async Task<T> Get<T>(long Id = 0) where T : IEntity => Id == 0 
            ? _context.Set<T>().AsNoTracking().FirstOrDefault() 
            : _context.Set<T>().AsNoTracking().FirstOrDefault(t => t.Id == Id);

        public async Task<List<T>> Get<T>() where T : class => await _context.Set<T>().ToListAsync();

        public async Task Add<T>(T item) where T : class
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update<T>(T item) where T : class
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Remove<T>(T item) where T : class
        {
            _context.Set<T>().Remove(item);
            await _context.SaveChangesAsync();
        }

    }
}
