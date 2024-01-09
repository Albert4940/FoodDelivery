namespace FoodDeliveryAPI.Models
{
    public class UserConstants
    {
        public static List<User> Users = new()
        {
            new (){ UserName = "albert_admin", Password="1234"},
            new(){ UserName = "dorce_admin", Password="1234"}
        };
    }
}
