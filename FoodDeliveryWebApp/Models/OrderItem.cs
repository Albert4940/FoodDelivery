namespace FoodDeliveryWebApp.Models
{
    public class OrderItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long Qty { get; set; } = 0;
        public string ImageURL { get; set; }

        public double Price { get; set; }

        public long ProductId { get; set; }
        public long CartId {  get; set; }
        public long CountInStock { get; set; }
    }
}
