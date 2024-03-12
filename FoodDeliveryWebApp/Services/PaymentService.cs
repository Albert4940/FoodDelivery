using FoodDeliveryWebApp.Models;
using System.Text.Json.Nodes;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryWebApp.Services
{
    public class PaymentService
    {
        public string PayPalCientId { get; set; } = "";
        public string PayPalSecret { get; set; } = "";
        public string PayPalUrl { get; set; } = "";
        public PaymentService(IConfiguration configuration)
        {
            PayPalCientId = configuration["PayPalSettings:ClientId"];
            PayPalSecret = configuration["PayPalSettings:SecretKey"];

            //get that dynamicly
            PayPalUrl = configuration["PayPalSettings:UrlSandBox"];
        }

        public string CreateOrder(double orderAmount)
        {
            string orderId = string.Empty;

            JsonObject createOrderRequest = new JsonObject();
            createOrderRequest.Add("intent", "CAPTURE");

            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "USD");
            amount.Add("value", orderAmount);

            JsonObject purchaseUnit1 = new JsonObject();
            purchaseUnit1.Add("amount", amount);

            JsonArray purchaseUnits = new JsonArray();
            purchaseUnits.Add(purchaseUnit1);

            createOrderRequest.Add("purchase_units", purchaseUnits);

            string accessToken = GetPayPalAccessToken();
            string url = PayPalUrl + "/v2/checkout/orders";

            
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(createOrderRequest.ToString(), null, "application/json");

                var responseTask = client.SendAsync(requestMessage);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var strResponse = readTask.Result;
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse is not null)
                    {
                        orderId = jsonResponse["id"]?.ToString() ?? "";

                        //save the order id in the database
                    }

                }
            }
            return orderId;
        }

        /*
            get tansaction id and payer email address if payment is completed 
            and return payment in order to add it in database
        */
        public Payment CompleteOrder(string OrderID)
        {
            Payment Payment = null;

            string accessToken = GetPayPalAccessToken();

            string url = $"{PayPalUrl}/v2/checkout/orders/{OrderID}/capture";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("", null, "application/json");


                var responseTask = client.SendAsync(requestMessage);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var strResponse = readTask.Result;

                    var jsonResponse = JsonNode.Parse(strResponse);

                    if (jsonResponse is not null)
                    {
                        string paypalOrderStatus = jsonResponse["status"]?.ToString() ?? "";
                        string PayerEmailAddress = jsonResponse["payer"]["email_address"]?.ToString() ?? "";
                        var transactionID = jsonResponse["purchase_units"][0]["payments"]["captures"][0]["id"]?.ToString();

                        if (paypalOrderStatus == "COMPLETED")
                        {                          
                            return new Payment {
                                Status = paypalOrderStatus,
                                TransactionID = transactionID,
                                EmailAddress = PayerEmailAddress
                            };
                        }
                    }
                }
            }
            return Payment;
        }
        public string GetPayPalAccessToken()
        {
            var accessToken = string.Empty;
            string url = PayPalUrl + "/v1/oauth2/token";

            using (var client = new HttpClient())
            {
                string credentials64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(PayPalCientId + ":" + PayPalSecret));


                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials64);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("grant_type=client_credentials", null, "application/x-www-form-urlencoded");


                var responseTask = client.SendAsync(requestMessage);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var strResponse = readTask.Result;

                    var jsonResponse = JsonNode.Parse(strResponse);

                    if (jsonResponse is not null)
                        accessToken = jsonResponse["access_token"]?.ToString() ?? "";
                }
            }
            return accessToken;
        }
    }
}
