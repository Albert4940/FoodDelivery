using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using FoodDeliveryWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json.Nodes;

namespace FoodDeliveryWebApp.Controllers
{
    public class CheckoutController : Controller
    {
        /*public string PayPalCientId { get; set; } = "";
        private string PayPalSecret { get; set; } = "";
        public string PayPalUrl { get; set; } = "";*/

        private OrderAPIService _orderAPIService;
        private readonly BaseService _baseService;
        private PaymentService _paymentService;
        private readonly Payment _payment;
        private  OrderViewModel _order;
        //public Order 
        public CheckoutController(IConfiguration configuration, FoodDeliveryWebAppDbContext context, IHttpClientFactory httpClientFactory) 
        {
            /*PayPalCientId = configuration["PayPalSettings:ClientId"];
            PayPalSecret = configuration["PayPalSettings:SecretKey"];

            //get that dynamicly
            PayPalUrl = configuration["PayPalSettings:UrlSandBox"];*/

            _orderAPIService = new OrderAPIService(httpClientFactory);
            _baseService = new BaseService(context);

            _payment = new Payment(configuration["PayPalSettings:ClientId"]);
            _paymentService = new PaymentService(configuration);
        }
        // GET: CheckoutController
        public async Task<ActionResult> Index(long Id = 0)
        {
           // TempData["Result"] = PayPalCientId;

            try
            {
                var token = HttpContext.Session.GetString("JWToken");

                /*if (token is null || token == "")
                    return Redirect("/User/Index?redirect=Order");*/

                //var Order = await OrderService.Get(OrderId, token);

                //var Order = await _orderAPIService.Get<OrderViewModel>(Id, token);
                _order = await _orderAPIService.Get<OrderViewModel>(Id, token);

                // Order.ShippingAddress = ShippingAddressService.Get();
                _order.ShippingAddress = await _baseService.Get<ShippingAddress>(0);
                _order.Payment = _payment;
                return _order is null ? View() : View(_order);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error INDEX: {ex.Message}";
            }
            
            return View();
        }

        public async Task<OrderViewModel> GetOrder(long Id = 0)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");

                /*if (token is null || token == "")
                    return Redirect("/User/Index?redirect=Order");*/

                //var Order = await OrderService.Get(OrderId, token);

                //var Order = await _orderAPIService.Get<OrderViewModel>(Id, token);
                _order = await _orderAPIService.Get<OrderViewModel>(Id, token);

                // Order.ShippingAddress = ShippingAddressService.Get();
                _order.ShippingAddress = await _baseService.Get<ShippingAddress>(0);
                _order.Payment = _payment;
                return _order;
            }
            catch (Exception ex)
            {
                throw;
            }

            return null;
        }
        // GET: CheckoutController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // Post: CheckoutController/Create
        public async Task<JsonResult> Create([FromBody] JsonObject data)
        {
            //check address

            /*JsonObject createOrderRequest = new JsonObject();
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
            }*/

            //return new JsonResult($"ClientID-{_paymentService.PayPalCientId} - PayPalSecret-{_paymentService.PayPalSecret} PayPalUrl-{_paymentService.PayPalUrl}");

            if (long.TryParse(data["Id"].ToString(), out long OrderId))
            {
                var OrderUser = await GetOrder(OrderId);
                var orderAmount = OrderUser.Order.TotalPrice;
                var orderPaypalId = _paymentService.CreateOrder(orderAmount);

                var response = new
                {
                    Id = orderPaypalId
                };

                return new JsonResult(response);
            }
            else
                return new JsonResult("");              
  
        }
        public JsonResult Complete([FromBody] JsonObject data)
        {
            if(data is null || data["orderID"] is null) return new JsonResult("");

            var orderID = data["orderID"]!.ToString();


            /*string accessToken = GetPayPalAccessToken();

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
            }*/
            //pass payment to update field if success
            var response = _paymentService.CompleteOrder(orderID) ?? "";
            
            
            return new JsonResult(response);
        }

        public JsonResult Cancel([FromBody] JsonObject data)
        {
            if (data is null || data["orderID"] is null) return new JsonResult("");

            var orderID = data["orderID"]!.ToString();

            return new JsonResult("");
        }
      
    }
}
