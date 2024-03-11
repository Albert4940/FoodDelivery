namespace FoodDeliveryAPI.Models
{
    public class Payment : BaseEntity
    {
        public long OrderId { get; set; }
        public string TransactionID { get; set; }
        public string Status { get; set; }

        //search how can i add this check contraint with ef
        // public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Paypal;
        public string PaymentMethod { get; set; } = "PayPal";
        public string EmailAddress { get; set; } = null;
    }

   // public enum PaymentMethod { Paypal,CreditCard,Other}
}
