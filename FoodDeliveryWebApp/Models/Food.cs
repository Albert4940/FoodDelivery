namespace FoodDeliveryWebApp.Models
{
    public class Food
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double Calories { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        //change it to long

        public long CategoryId { get; set; }


        public string UserId { get; set; }
        // public virtual Category? category { get; set; }
    }
}
