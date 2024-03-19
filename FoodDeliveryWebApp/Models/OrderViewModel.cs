namespace FoodDeliveryWebApp.Models
{
    public class OrderViewModel
    {
        public User User { get; set; } = null;
        public List<OrderItem> OrderItems { get; set; }
        public ShippingAddress ShippingAddress { get; set; }

        public Payment Payment { get; set; }

        public Order Order { get; set; }
        public Configuration Configuration { get; set; }
        //user
    }
}
