namespace FoodDeliveryWebApp.Models
{
    public class Order : IEntity
    {
        //public List<OrderItem> OrderItems { get; set; }
       // public ShippingAddress ShippingAddress { get; set; }
        

        public long Id { get; set; }
        public string UserId { get; set; } = "string";
        public double ItemsPrice { get; set; }

        public double TaxPrice { get; set; }

        public double ShippingPrice {  get; set; }
        public double TotalPrice { get; set;}
        public bool IsDelivered { get; set; } = false;
        //public bool IsPaid { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        //public string PaymentMethod { get; set; }
    }
}
