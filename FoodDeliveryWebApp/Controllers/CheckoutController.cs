using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json.Nodes;

namespace FoodDeliveryWebApp.Controllers
{
    public class CheckoutController : Controller
    {
        public string PayPalCientId { get; set; } = "";
        private string PayPalSecret { get; set; } = "";
        public string PayPalUrl { get; set; } = "";

        //public Order 
        public CheckoutController(IConfiguration configuration) 
        {
            PayPalCientId = configuration["PayPalSettings:ClientId"];
            PayPalSecret = configuration["PayPalSettings:SecretKey"];

            //get that dynamicly
            PayPalUrl = configuration["PayPalSettings:UrlSandBox"];

        }
        // GET: CheckoutController
        public ActionResult Index()
        {
            TempData["Result"] = PayPalCientId;
            return View();
        }

        // GET: CheckoutController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // Post: CheckoutController/Create
        public JsonResult Create()
        {
            //check address

            JsonObject createOrderRequest = new JsonObject();
            createOrderRequest.Add("intent", "CAPTURE");

            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "USD");
            amount.Add("value", 40);

            JsonObject purchaseUnit1 = new JsonObject();
            purchaseUnit1.Add("amount", amount);

            JsonArray purchaseUnits = new JsonArray();
            purchaseUnits.Add(purchaseUnit1);

            createOrderRequest.Add("purchase_units", purchaseUnits);

            string accessToken = GetPayPalAccessToken();
            string url = PayPalUrl + "/v2/checkout/orders";

            string orderId = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(createOrderRequest.ToString(), null, "application/json");

                var responseTask = client.SendAsync(requestMessage);
                responseTask.Wait();

                var result = responseTask.Result;
                if(result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var strResponse = readTask.Result;
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if(jsonResponse is not null)
                    {
                        orderId = jsonResponse["id"]?.ToString() ?? "";

                        //save the order id in the database
                    }

                }
            }


            var response = new
            {
                Id = orderId
            };
            
            return new JsonResult(response);
        }
        public JsonResult Complete([FromBody] JsonObject data)
        {
            if(data is null || data["orderID"] is null) return new JsonResult("");

            var orderID = data["orderID"]!.ToString();

            string accessToken = GetPayPalAccessToken();

            string url = $"{PayPalUrl}/v2/checkout/orders/{orderID}/capture";

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
                        if (paypalOrderStatus == "COMPLETED")
                        {
                            //update payment status in the databse

                            return new JsonResult("success");
                        }
                    }                        
                }
            }

            return new JsonResult("");
        }

        public JsonResult Cancel([FromBody] JsonObject data)
        {
            if (data is null || data["orderID"] is null) return new JsonResult("");

            var orderID = data["orderID"]!.ToString();

            return new JsonResult("");
        }

        private string GetPayPalAccessToken()
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
