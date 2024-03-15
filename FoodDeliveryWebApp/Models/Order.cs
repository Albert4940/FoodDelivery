namespace FoodDeliveryWebApp.Models
{
    public class Order : IEntity
    {
        public long Id { get; set; }
        public string UserId { get; set; } = "string";
        public decimal ItemsPrice { get; set; }

        public decimal TaxPrice { get; set; }

        public decimal DeliveryFee {  get; set; }
        public decimal TotalPrice { get; set;}
        public bool IsDelivered { get; set; } = false;
        public bool IsPaid { get; set; } = false;
    }
}
