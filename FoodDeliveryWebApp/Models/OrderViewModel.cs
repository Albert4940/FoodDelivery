namespace FoodDeliveryWebApp.Models
{
    public class OrderViewModel
    {
        public List<OrderItem> OrderItems { get; set; }
        public ShippingAddress ShippingAddress { get; set; }

        public Payment Payment { get; set; }

        public Order Order { get; set; }
        //user
    }
}
