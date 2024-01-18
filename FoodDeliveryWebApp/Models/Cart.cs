namespace FoodDeliveryWebApp.Models
{
    public class Cart
    {
        public List<OrderItem> OrderItems { get; set; }
        public ShippingAddress ShippingAddress { get; set; }

        public string PaymentMethod { get; set; }

        public double ItemsPrice { get; set; }

        public double TaxPrice { get; set; }

        public double ShippingPrice {  get; set; }
        public double TotalPrice { get; set;}
    }
}
