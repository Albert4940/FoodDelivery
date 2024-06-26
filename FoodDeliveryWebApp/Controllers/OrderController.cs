﻿using FoodDeliveryWebApp.Data;
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
    [SessionExpire]
    public class OrderController : Controller
    {
        private OrderAPIService _orderAPIService;
        private readonly BaseAPIService _baseAPIService;
        private readonly UserAPIService _userAPIService;
        private readonly BaseService _baseService;
        private readonly CartService _cartService;
        private readonly OrderItemService _orderItemService;
        public OrderController(FoodDeliveryWebAppDbContext context, IHttpClientFactory httpClientFactory)
        {
            //FoodService.InitailizeHttp(httpClientFactory);
            //CartService.InintializeContextDb(context);
            //OrderItemService.InintializeContextDb(context);
           // ShippingAddressService.InintializeContextDb(context);

            _orderAPIService = new OrderAPIService(httpClientFactory);
            _baseAPIService = new BaseAPIService(httpClientFactory);
            _userAPIService = new UserAPIService(httpClientFactory);

            _baseService = new BaseService(context);
            _cartService = new CartService(context);
            _orderItemService = new OrderItemService(context);

            //BaseAPIService.InitailizeHttp(httpClientFactory);
            //OrderService.InitailizeHttp(httpClientFactory);
        }
       
        public async Task<ActionResult> Index()
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");
                var Orders = await _orderAPIService.Get<Order>(token);
                return View(Orders);
            }catch(Exception ex)
            {
                TempData["Error"] = ex;
            }
            return View();
        }

        // GET: OrderController
       
        public async Task<ActionResult> Details(long Id = 0)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");

                /*if (token is null || token == "")
                    return Redirect("/User/Index?redirect=Order");*/

                //var Order = await OrderService.Get(OrderId, token);

                var Order = await _orderAPIService.Get<OrderViewModel>(Id, token);

                // Order.ShippingAddress = ShippingAddressService.Get();
                //Order.ShippingAddress = await _baseService.Get<ShippingAddress>(0);
                return Order is null ? View() : View(Order);
            }catch(Exception ex)
            {
                TempData["Error"] = $"Error INDEX: {ex.Message}";
            }
            
            return View();
        }

        
        public async Task<IActionResult> Address()
        {
            //var model = TempData["PaymentMethod"] as Payment;
            /*if (TempData["PaymentMethod"] is null || TempData["PaymentMethod"] == "")
            {
                return RedirectToAction("PaymentMethod");
            }*/
            var UserId = HttpContext.Session.GetString("UserId");

            try
            {
                var Address = await _userAPIService.GetUserShippingAddress(UserId);
                return View(Address);
            }catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
            }
            return View();
        }

        //[SessionExpire]
        [HttpPost]
        public async Task<IActionResult> Address(ShippingAddress model)
        {
            /*if (!ModelState.IsValid)
            {
                TempData["Error"] = "Error";
                return View(model);
            }*/

            var UserId = HttpContext.Session.GetString("UserId");       
            var Address = await _userAPIService.GetUserShippingAddress(UserId);

            try
            {
                
                if(Address is not null)
                {
                    if (Address.Id == model.Id)
                    {
                        //await ShippingAddressService.Update(model);
                        model.UserId = UserId;
                        await _baseAPIService.Update<ShippingAddress>(model.Id,model);
                    }
                }
                else
                {
                    //await ShippingAddressService.Add(model);
                    model.UserId = UserId ?? null; 
                    await _baseAPIService.Add<ShippingAddress>(model);
                }

                var OrderResult = await CreateOrder(UserId);

                return Redirect($"~/Checkout/Index/{OrderResult.Id}");
                //return RedirectToAction("Create");
            }
            catch(Exception ex)
            {
                TempData["Error"] = $"{ex.Message.ToString()}";
                return View(model);
            }
        }

        private async Task<Order> CreateOrder(string UserId)
        {
            //try
            //{
                var token = HttpContext.Session.GetString("JWToken");

                var ShippingAddress = await _userAPIService.GetUserShippingAddress(UserId);

                var CartOrder = await _cartService.Get(UserId);

                var OrderItems = await _orderItemService.Get(CartOrder.Id);

                var model = new OrderViewModel
                {
                    OrderItems = OrderItems,
                    ShippingAddress = ShippingAddress,
                    Order = CartOrder
                };

                var OrderResult = await _orderAPIService.Add(model, token);
            //remove specific cart and orderitem 
            await _baseService.RemoveRange<OrderItem>(OrderItems);
                await _baseService.Remove<Order>(CartOrder);

                return OrderResult;
                //return RedirectToAction("Details", new { Id = OrderResult.Id });
           /* }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
            }

            return null;*/
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
                    Order = await _baseService.Get<Order>(0)
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
                    Order = CartOrder
                };

                //var OrderResult = await OrderService.Add(model, token);
                var OrderResult = await _orderAPIService.Add(model,token);
                //TempData["Result"] = $"{OrderResult.Id.ToString()}-{OrderResult.UserId.ToString()}-{OrderResult.TotalPrice.ToString()}-{OrderResult.ItemsPrice.ToString()}";

                //remove cart
               // await _baseService.Remove<OrderItem>();
                //await _baseService.Remove<Order>();
                

                return RedirectToAction("Details", new { Id = OrderResult.Id });
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

        [HttpPost]
        public async Task<ActionResult> Delete(long Id)
        {
            /* try
             {
                 await _baseAPIService.Delete<Order>(Id);
             }
             catch(Exception ex)
             {
                 TempData["Error"] = ex.Message.ToString();
             }*/
            TempData["Error"] = Id.ToString();
            return RedirectToAction(nameof(Index));
        }

    }
}
