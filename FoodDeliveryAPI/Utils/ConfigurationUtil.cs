namespace FoodDeliveryAPI.Utils
{
    public static class ConfigurationUtil
    {
        public static Object GetConfiguration(IConfiguration _configuration)
        {
            //trhow error
            var DeliveryFeeConfig = _configuration["OrderSettings:DeliveryFee"].ToString();
            var TaxPercentConfig = _configuration["OrderSettings:TaxPercent"].ToString();

            if (!decimal.TryParse(DeliveryFeeConfig, out decimal DeliveryFee))
                DeliveryFee = 0m;

            if (!int.TryParse(TaxPercentConfig, out int TaxPercent))            
                TaxPercent = 0;
            


            return new
            {
                PayPalCientId = _configuration["PayPalSettings:ClientId"].ToString(),
                PayPalSecret = _configuration["PayPalSettings:SecretKey"].ToString(),
                PayPalUrl = _configuration["PayPalSettings:Url"].ToString(),
                DeliveryFee,
                TaxPercent
            };
        }
    }
}
