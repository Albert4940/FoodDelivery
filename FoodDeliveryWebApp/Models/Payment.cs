namespace FoodDeliveryWebApp.Models
{
    public class Payment : IEntity
    {
        public long Id { get; set; }
        public string[] PaymentMethodes { get; set; } = new string[] { "Paypal", "CreditCard" };
        public string PaymentMethodSelected { get; set; } = "PayPal";
    }

}
