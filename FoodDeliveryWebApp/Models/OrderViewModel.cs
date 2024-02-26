namespace FoodDeliveryWebApp.Models
{
    public class OrderViewModel
    {
        public List<OrderItem> OrderItems { get; set; }
        public ShippingAddress ShippingAddress { get; set; }

        //public string PaymentMethod { get; set; }

        public Order cart { get; set; }
    }
}
