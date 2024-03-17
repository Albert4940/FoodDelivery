namespace FoodDeliveryAPI.Models
{
    public class OrderRequest
    {
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
    }
}
