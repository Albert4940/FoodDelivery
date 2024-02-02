namespace FoodDeliveryWebApp.Models
{
    public class Order
    {
        public long Id { get; set; }
        public string UserId { get; set; }

        public double ItemsPrice { get; set; }
        public double TaxPrice { get; set; }        
        public decimal TotalPrice { get; set; }

        public string PaymentMethod { get; set; }
        public bool IsPaid { get; set; } = false;
        public bool IsDelivered { get; set; } = false;
       // public 
    }
}
