using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryAPI.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //change it to long 
        public long Id { get; set; }

        public string Title { get; set; }
        public string IconUrl { get; set; }

        
        public string UserId { get; set; }

    }
}
