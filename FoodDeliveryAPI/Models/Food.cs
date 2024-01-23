using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryAPI.Models
{
    public class Food
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double Calories { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public long CountInStock { get; set; }

        public long CategoryId { get; set; }

       
        public string UserId { get; set; }
        // public virtual Category? category { get; set; }
    }
}
