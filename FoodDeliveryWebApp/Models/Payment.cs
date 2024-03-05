namespace FoodDeliveryWebApp.Models
{
    public class Payment : IEntity
    {
        public long Id { get; set; }
        public string[] PaymentMethodes { get; set; } = new string[] { "Paypal", "CreditCard" };
        public string PaymentMethodSelected { get; set; } = "PayPal";
        public string PayPalCientId { get; set; } = "";
        private string PayPalSecret { get; set; } = "";
        public string PayPalUrl { get; set; } = "";
    }

}
