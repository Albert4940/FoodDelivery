namespace FoodDeliveryAPI.Models
{
    public class Configuration
    {
        public string PayPalCientId { get; set; } = "";
        public string PayPalSecret { get; set; } = "";
        public string PayPalUrl { get; set; } = "";
        public decimal DeliveryFee { get;  set; }
        public int TaxPercent { get; set; }
    }
}
