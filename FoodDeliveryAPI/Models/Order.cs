namespace FoodDeliveryAPI.Models
{
    public class Order : BaseEntity
    {
        public long Id { get; set; }
        public string UserId { get; set; } = "string";

        public decimal ItemsPrice { get; set; }
        public decimal TaxPrice { get; set; } 
        public decimal DeliveryFee { get; set; }
        public decimal TotalPrice { get; set; }

        public bool IsPaid { get; set; } = false;
        public bool IsDelivered { get; set; } = false;
       // public 
    }
}
