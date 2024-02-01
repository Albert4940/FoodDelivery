namespace FoodDeliveryAPI.Models
{
    public class OrderItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long Qty { get; set; } = 0;
        public string ImageURL { get; set; }

        public decimal Price { get; set; }

        public long ProductId { get; set; }
        public long OrderId {  get; set; }
        public long CountInStock { get; set; }
    }
}
