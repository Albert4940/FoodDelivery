using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using FoodDeliveryWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;
using System.Text;
using System.Text.Json.Nodes;

namespace FoodDeliveryWebApp.Controllers
{
    [SessionExpire]
    public class CheckoutController : Controller
    {
        /*public string PayPalCientId { get; set; } = "";
        private string PayPalSecret { get; set; } = "";
        public string PayPalUrl { get; set; } = "";*/
        private BaseAPIService _baseAPIService;
        private OrderAPIService _orderAPIService;
        private readonly BaseService _baseService;
        private PaymentService _paymentService;
        private readonly Payment _payment;
        //remove it and use it locally
        private  OrderViewModel _order;
        private string PayPalCientId;
        //public Order 
        public CheckoutController(IConfiguration configuration, FoodDeliveryWebAppDbContext context, IHttpClientFactory httpClientFactory) 
        {
            /*PayPalCientId = configuration["PayPalSettings:ClientId"];
            PayPalSecret = configuration["PayPalSettings:SecretKey"];

            //get that dynamicly
            PayPalUrl = configuration["PayPalSettings:UrlSandBox"];*/
            _baseAPIService = new BaseAPIService(httpClientFactory);
            _orderAPIService = new OrderAPIService(httpClientFactory);
            _baseService = new BaseService(context);

            //Get this from api instead from setting
            _payment = new Payment { PayPalCientId = configuration["PayPalSettings:ClientId"] };

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

                _order.Payment = _order.Order.IsPaid ? null : _payment;

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
                _order.Payment = new Payment { OrderId = _order.Order.Id};
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
            if (long.TryParse(data["Id"].ToString(), out long OrderId))
            {
                var OrderUser = await GetOrder(OrderId);

                //check if order already paid and return Error
                if(OrderUser.Order.IsPaid)
                    return new JsonResult(new { error = "Order is already paid!" });

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
        public async Task<JsonResult> Complete([FromBody] JsonObject data)
        {
            string response = string.Empty;

            if(data is null || data["orderID"] is null) return new JsonResult("");

            var orderPaymentID = data["orderID"]!.ToString();

            if(long.TryParse(data["orderUserID"].ToString(), out long OrderUserID))
            {                              

                //get payment oject if payment complete in order to add it in db
                var result = _paymentService.CompleteOrder(orderPaymentID) ?? null;
                var OrderViewModel = await GetOrder(OrderUserID);
                var OrderUser = OrderViewModel.Order;

                if(result is not null && OrderViewModel is not null)
                {
                    var Payment = result;
                    //fix it api side
                    Payment.Id = 0;
                    Payment.OrderId = OrderUserID;
                    //get it from data request
                    Payment.PaymentMethod = "PayPal";

                    //store to api
                    OrderUser.IsPaid = true;

                    //Add try catch
                    await _baseAPIService.Update<Order>(OrderUser.Id, OrderUser);
                    await _baseAPIService.Add<Payment>(Payment);

                    response = "success";
                }
               
            }

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
