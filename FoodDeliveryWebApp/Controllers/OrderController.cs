using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using FoodDeliveryWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FoodDeliveryWebApp.Controllers
{
    public class OrderController : Controller
    {
        private OrderAPIService _orderAPIService;
        public OrderController(FoodDeliveryWebAppDbContext context, IHttpClientFactory httpClientFactory)
        {
            FoodService.InitailizeHttp(httpClientFactory);
            CartService.InintializeContextDb(context);
            OrderItemService.InintializeContextDb(context);
            ShippingAddressService.InintializeContextDb(context);
            _orderAPIService = new OrderAPIService(httpClientFactory);

            //BaseAPIService.InitailizeHttp(httpClientFactory);
            OrderService.InitailizeHttp(httpClientFactory);
        }

        // GET: OrderController
        public async Task<ActionResult> Index(long OrderId = 0)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");

                if (token is null || token == "")
                    return Redirect("/User/Index?redirect=Order");

                var Order = await OrderService.Get(OrderId, token);
               // var Order = await _orderAPIService.Get<CartViewModel>(OrderId, token);
                Order.ShippingAddress = ShippingAddressService.Get();

                return Order is null ? View() : View(Order);
            }catch(Exception ex)
            {
                TempData["Error"] = $"Error INDEX: {ex.Message}";
            }
            
            return View();
        }

        public IActionResult PaymentMethod()
        {
            
            var model = new Payment();
            return View(model);
        }

        [HttpPost]
        public IActionResult PaymentMethod(Payment model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validate and process payment method data
            TempData["Result"] = model.PaymentMethodSelected.ToString();
            TempData["PaymentMethod"] = model.PaymentMethodSelected.ToString();

            //return View(method);
            return RedirectToAction("Address");
        }

        public IActionResult Address()
        {
            //var model = TempData["PaymentMethod"] as Payment;
            if (TempData["PaymentMethod"] is null || TempData["PaymentMethod"] == "")
            {
                return RedirectToAction("PaymentMethod");
            }

            try
            {
                var ShippingAddress = ShippingAddressService.Get();
                return View(ShippingAddress);
            }catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Address(ShippingAddress model)
        {
            /*if (!ModelState.IsValid)
            {
                TempData["Error"] = "Error";
                return View(model);
            }*/

            var Address = ShippingAddressService.Get();

            try
            {
                if(Address.Id == model.Id)
                {
                    await ShippingAddressService.Update(model);
                }
                else
                {
                    await ShippingAddressService.Add(model);
                }
                return RedirectToAction("Create");
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return View(model);
            }
        }

        // GET: OrderController/Create
        public async Task<ActionResult> Create()
        {
            try {
                var token = HttpContext.Session.GetString("JWToken");
                if (token is null || token == "")
                    return Redirect("/User/Index?redirect=Order");

                var Address = ShippingAddressService.Get();

                return View(new CartViewModel { 
                    OrderItems = await OrderItemService.GetAll(), 
                    ShippingAddress = Address, 
                    cart = CartService.Get()
                });

            }catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
            }

            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CartViewModel Order)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");
                
                var CartOrder = CartService.Get();
                var ShippingAddress = ShippingAddressService.Get();
                
                CartOrder.UserId = "string";
                ShippingAddress.Id = 0;
                ShippingAddress.UserId = "string";

                var model = new CartViewModel
                {
                    OrderItems = await OrderItemService.GetAll(),
                    ShippingAddress = ShippingAddress,
                    cart = CartOrder
                };

                 //var OrderResult = await OrderService.Add(model, token);
                var OrderResult = await _orderAPIService.Add(model,token);
                //TempData["Result"] = $"{OrderResult.Id.ToString()}-{OrderResult.UserId.ToString()}-{OrderResult.TotalPrice.ToString()}-{OrderResult.ItemsPrice.ToString()}";
                return RedirectToAction("Index", new { OrderId = OrderResult.Id });
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
            }

            return RedirectToAction(nameof(Create));
        }

    }
}
