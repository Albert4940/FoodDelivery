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
        private readonly BaseService _baseService;

        public OrderController(FoodDeliveryWebAppDbContext context, IHttpClientFactory httpClientFactory)
        {
            FoodService.InitailizeHttp(httpClientFactory);
            CartService.InintializeContextDb(context);
            //OrderItemService.InintializeContextDb(context);
            ShippingAddressService.InintializeContextDb(context);

            _orderAPIService = new OrderAPIService(httpClientFactory);
            _baseService = new BaseService(context);
            //BaseAPIService.InitailizeHttp(httpClientFactory);
            OrderService.InitailizeHttp(httpClientFactory);
        }

        /*public async Task<ActionResult> Index()
        {
            try
            {
                await _baseService.Get<>
            }catch(Exception ex)
            {
                TempData["Error"] = ex;
            }
        }*/

        // GET: OrderController
        [SessionExpire]
        public async Task<ActionResult> Details(long OrderId = 0)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");

                /*if (token is null || token == "")
                    return Redirect("/User/Index?redirect=Order");*/

                //var Order = await OrderService.Get(OrderId, token);

                var Order = await _orderAPIService.Get<OrderViewModel>(OrderId, token);

                // Order.ShippingAddress = ShippingAddressService.Get();
                Order.ShippingAddress = await _baseService.Get<ShippingAddress>(0);
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

        public async Task<IActionResult> Address()
        {
            //var model = TempData["PaymentMethod"] as Payment;
            if (TempData["PaymentMethod"] is null || TempData["PaymentMethod"] == "")
            {
                return RedirectToAction("PaymentMethod");
            }

            try
            {
                //var ShippingAddress = ShippingAddressService.Get();
                var ShippingAddress = await _baseService.Get<ShippingAddress>(0);
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

            // var Address = ShippingAddressService.Get();
            var Address = await _baseService.Get<ShippingAddress>(0);

            try
            {
                if(Address.Id == model.Id)
                {
                    //await ShippingAddressService.Update(model);
                    model.UserId = "string";
                    await _baseService.Update<ShippingAddress>(model);
                }
                else
                {
                    //await ShippingAddressService.Add(model);
                    model.UserId = "string";
                    await _baseService.Add<ShippingAddress>(model);
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

                //var Address = ShippingAddressService.Get();
                var Address = await _baseService.Get<ShippingAddress>(0);
                /* return View(new CartViewModel { 
                     OrderItems = await OrderItemService.GetAll(), 
                     ShippingAddress = Address, 
                     cart = CartService.Get()
                 });*/
                return View(new OrderViewModel
                {
                    OrderItems = await _baseService.Get<OrderItem>(),
                    ShippingAddress = Address,
                    cart = await _baseService.Get<Order>(0)
                });
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
            }

            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrderViewModel Order)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");

                //var CartOrder = CartService.Get();
                //var ShippingAddress = ShippingAddressService.Get();

                var ShippingAddress = await _baseService.Get<ShippingAddress>(0);
                var CartOrder = await _baseService.Get<Order>(0);

                CartOrder.UserId = "string";
                ShippingAddress.Id = 0;
                ShippingAddress.UserId = "string";

                /*var model = new CartViewModel
                {
                    OrderItems = await OrderItemService.GetAll(),
                    ShippingAddress = ShippingAddress,
                    cart = CartOrder
                };*/
                var model = new OrderViewModel
                {
                    OrderItems = await _baseService.Get<OrderItem>(),
                    ShippingAddress = ShippingAddress,
                    cart = CartOrder
                };

                //var OrderResult = await OrderService.Add(model, token);
                var OrderResult = await _orderAPIService.Add(model,token);
                //TempData["Result"] = $"{OrderResult.Id.ToString()}-{OrderResult.UserId.ToString()}-{OrderResult.TotalPrice.ToString()}-{OrderResult.ItemsPrice.ToString()}";

                //remove cart
                await _baseService.Remove<OrderItem>();
                await _baseService.Remove<Order>();
                

                return RedirectToAction("Details", new { OrderId = OrderResult.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
            }
            /*catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
            }*/

            return RedirectToAction(nameof(Create));
        }

    }
}
