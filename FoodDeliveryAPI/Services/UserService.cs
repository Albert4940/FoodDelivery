using FoodDeliveryAPI.Models;

namespace FoodDeliveryAPI.Services
{
    public static class UserService
    {
        public static List<User> Users = new()
        {
            new (){ UserName = "albert_admin", Password="1234"},
            new(){ UserName = "dorce_admin", Password="1234"}
        };

        public static User? Get(User user) => Users.FirstOrDefault(x => x.UserName.Equals(user.UserName, StringComparison.CurrentCultureIgnoreCase)
            && x.Password == user.Password);
    }
}
