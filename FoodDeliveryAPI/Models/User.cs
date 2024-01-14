using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryAPI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        //Make it unique in database
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
