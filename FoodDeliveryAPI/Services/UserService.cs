using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace FoodDeliveryAPI.Services
{
    public static class UserService
    {
        /*private static readonly FoodDeliveryContext _context;
        Add Try exception
        public static void setDbContext(FoodDeliveryContext context) => _context = context;*/
        private static FoodDeliveryContext _context;

        // Method to initialize the context
        public static void InitializeContext(FoodDeliveryContext context)
        {
            _context = context;
        }


        /*public static User? Get(User user) => Users.FirstOrDefault(x => x.UserName.Equals(user.UserName, StringComparison.CurrentCultureIgnoreCase)
            && x.Password == user.Password);*/
        public static async Task<User> Get(User user) => await _context.users.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Password == user.Password);
        public static async Task<User> GetByID(string userID) => await _context.users.AsNoTracking().FirstOrDefaultAsync(u => u.Id.Equals(userID));

        public static async Task<List<User>> GetAll() => await _context.users.ToListAsync();
        public static async Task Add(User user) {
            //That generate by database    
                user.Id = null;
                _context.Add(user);
                await _context.SaveChangesAsync();
    } 

        public static async Task<User> Authenticate(User user)
        {
            // return await UserService.Get(user);
            //return await _context.users.SingleOrDefault(x => x.UserName == user.UserName);
             return await _context.users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
            // var currentUser = UserService.Get(user);
            /* var user = await _context.users
                  .FirstOrDefaultAsync(m => m.Id == id);

             if (currentUser != null) return currentUser;

             return null;*/
        }

        public static async Task<bool> CheckIfUserExists (User user) =>  await UserService.Get(user) != null;
        /*public static User GetCurrent()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    UserName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                };
            }
            return null;
        }*/
    }
}
