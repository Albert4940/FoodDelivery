namespace FoodDeliveryWebApp.Models
{
    public class OrderItem
    {
        public string Title { get; set; }
        public long Qty { get; set; } = 0;
        public string ImageURL { get; set; }

        public double Price { get; set; }

        public long ProductId { get; set; }
    }
}
