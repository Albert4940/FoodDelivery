namespace FoodDeliveryWebApp.Models
{
    public class CartViewModel
    {
        public List<OrderItem> OrderItems { get; set; }
        public ShippingAddress ShippingAddress { get; set; }

        //public string PaymentMethod { get; set; }

        public Cart cart { get; set; }
    }
}
