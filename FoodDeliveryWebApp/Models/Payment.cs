namespace FoodDeliveryWebApp.Models
{
    public class Payment : IEntity
    {
        public long Id { get; set; }
        public string[] PaymentMethodes { get; set; } = new string[] { "Paypal", "CreditCard" };
        public string PaymentMethodSelected { get; set; } = "PayPal";

        //Add this in paymentConfiguration class
        public string PayPalCientId { get; set; } = "";

        public Payment(string _payPalCientId)
        {
            PayPalCientId = _payPalCientId;
        }
    }

}
