namespace FoodDeliveryWebApp.Models
{
    public class Payment
    {
        public IEnumerable<PaymentMethodVm> PaymentMethodes { get; set; } = new List<PaymentMethodVm>
        {
            new PaymentMethodVm {Id = 1, PaymentMethod = "Paypal"},
            new PaymentMethodVm {Id = 2, PaymentMethod = "MasterCard"}
        };

        public string PaymentMethodSelected { get; set; } = "PayPal";
    }

    public class PaymentMethodVm
    {
        public int Id { set; get; }
        public string PaymentMethod { get; set;}
    }
}
