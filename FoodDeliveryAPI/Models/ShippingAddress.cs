namespace FoodDeliveryAPI.Models
{
    public class ShippingAddress : BaseEntity
    {
        public long Id { get; set; }
        public string UserId { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
