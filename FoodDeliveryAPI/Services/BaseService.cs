using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryAPI.Services
{
    public class BaseService
    {
        protected static FoodDeliveryContext _context;
        public BaseService(FoodDeliveryContext context)
        {
            _context = context;
        }

        public async Task<List<T>> Get<T>() where T : class => await _context.Set<T>().AsNoTracking().ToListAsync();
        public async Task<T> Get<T>(long id) where T : BaseEntity
        {
            var entity = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(t => t.Id == id) ?? null;
            DetachContext();
            return entity;
        }

        public async Task Add<T>(T item) where T : class
        {
            //That generate by database    
            //food.Id = 0;
            _context.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update<T>(T item) where T : class
        {
            _context.Set<T>().Update(item);
            await _context.SaveChangesAsync();
        }
        public async Task Remove<T>(T item) where T : class
        {
            _context.Set<T>().Remove(item);
            await _context.SaveChangesAsync();
        }
        private void DetachContext()
        {
            _context.ChangeTracker.Clear();
        }
    }
}
