namespace FoodDeliveryWebApp.Models
{
    public class Payment : IEntity
    {
        public long Id { get; set; }
        public string TransactionID { get; set; } = null;
        public string Status { get; set; } = null;

        public string EmailAddress { get; set; } = null;
        public long OrderId { get; set; } = 0;
        //Put this field into setting file
        public string[] PaymentMethodes { get; set; } = new string[] { "Paypal", "CreditCard" };
        public string PaymentMethod { get; set; } = "PayPal";

        //Add this in paymentConfiguration class
        //public string PayPalCientId { get; set; } = "";

        /*public Payment(string _payPalCientId)
        {
            PayPalCientId = _payPalCientId;
        }*/
    }

}
