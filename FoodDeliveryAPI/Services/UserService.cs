﻿using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace FoodDeliveryAPI.Services
{
    public static class UserService
    {
        /*private static readonly FoodDeliveryContext _context;
        Add Try exception
        public static void setDbContext(FoodDeliveryContext context) => _context = context;*/

        public static List<User> Users = new()
        {
            new (){ UserName = "albert_admin", Password="1234"},
            new(){ UserName = "dorce_admin", Password="1234"}
        };

        /*public static User? Get(User user) => Users.FirstOrDefault(x => x.UserName.Equals(user.UserName, StringComparison.CurrentCultureIgnoreCase)
            && x.Password == user.Password);*/
        public static async Task<User> Get(User user, FoodDeliveryContext context) => await context.users.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Password == user.Password);

        public static async Task Add(User user, FoodDeliveryContext context) {
            //That generate by database    
                user.Id = null;
                context.Add(user);
                await context.SaveChangesAsync();
    } 

        public static async Task<User> Authenticate(User user, FoodDeliveryContext _context)
        {
            return await UserService.Get(user, _context);
            // return await _context.users.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Password == user.Password);
            // var currentUser = UserService.Get(user);
            /* var user = await _context.users
                  .FirstOrDefaultAsync(m => m.Id == id);

             if (currentUser != null) return currentUser;

             return null;*/
        }

        public static async Task<bool> CheckIfUserExists (User user, FoodDeliveryContext _context) =>  await UserService.Get(user, _context) != null;
    }
}
