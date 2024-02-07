namespace FoodDeliveryWebApp.Models
{
    public class Payment
    {
        public string[] PaymentMethodes { get; set; } = new string[] { "Paypal", "CreditCard" };
        public string PaymentMethodSelected { get; set; } = "PayPal";
    }

}
