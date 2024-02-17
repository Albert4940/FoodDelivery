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

        public async Task<T> Get<T>(long Id = 0) where T : IEntity
        {
            var entity = Id == 0
                ? await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync()
                : await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(t => t.Id == Id);

            DetachContext();

            return entity;
        }

        public async Task<List<T>> Get<T>() where T : class => await _context.Set<T>().AsNoTracking().ToListAsync();

        public async Task Add<T>(T item) where T : class
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update<T>(T item) where T : class
        {
            //_context.Entry(item).State = EntityState.Modified;
            _context.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Remove<T>(T item) where T : class
        {
            _context.Set<T>().Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task Remove<T>() where T : class
        {
            var items = await Get<T>();

            _context.Set<T>().RemoveRange(items);
            await _context.SaveChangesAsync();
            DetachContext();
        }

        private void DetachContext()
        {
            _context.ChangeTracker.Clear();
        }
    }
}
