namespace FoodDeliveryWebApp.Models
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; } = null;
        public string Password { get; set; } = null;
        public string Token { get; set; }
    }
}
