﻿using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using FoodDeliveryWebApp.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Text;

namespace FoodDeliveryWebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly BaseAPIService _baseAPIService;
        private readonly BaseService _baseService;
        private readonly OrderItemService _orderItemService;
        private readonly CartService _cartService;
        public CartController(FoodDeliveryWebAppDbContext context, IHttpClientFactory httpClientFactory)
        {
           
           // FoodService.InitailizeHttp(httpClientFactory);
            //CartService.InintializeContextDb(context);
            //OrderItemService.InintializeContextDb(context);

            _baseAPIService = new BaseAPIService(httpClientFactory);
            _baseService = new BaseService(context);
            _orderItemService = new OrderItemService(context);
            _cartService = new CartService(context);
        }
        //Get
        [HttpGet]
        [SessionExpire]
        public async Task<ActionResult> Index()
        {
            try
            {
                //var cart = CartService.Get();
                //var cart = await _baseService.Get<Order>(0);

                //var UserId = HttpContext.Session.GetString("UserId");

                /*var carts = await _baseService.Get<Order>();
                var cart = carts.FirstOrDefault(c => c.UserId == UserId);*/

                //var orderItems = await OrderItemService.GetAll();


                /*if (UserId != null)
                {
                    var CartViewModel = await _cartService.Get(UserId);
                    return View(CartViewModel);
                }*/
                // var cart = await _cartService.Get(UserId);
                var cart = await GetCartByUserId();

                if (cart != null)
                {
                    var CartViewModel = new OrderViewModel()
                    {
                        Order = cart,
                        OrderItems = await _orderItemService.Get(cart.Id)
                    };
                    return View(CartViewModel);
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return View();
            }
            return View();
        }

        [HttpPost]
        [SessionExpire]
        // Post: CartController
        public async Task<ActionResult> Index(long Id, int CountInStock)
        {
            long FoodId = Id;
            int Qty = CountInStock;

            //Check both params
            if(FoodId <= 0 || Qty <= 0)
            {
                TempData["Error"] = "Bad request";
                return View();
            }

            try
            {
                await AddToCart(FoodId, Qty);
                await UpdateCartPrices();
            }catch(Exception ex)
            {               
                TempData["Error"] = $"Error : {ex}";
            }

            try
            {
                /*var cart = CartService.Get();
                var orderItems = await OrderItemService.GetAll();*/

                /*var cart = await _baseService.Get<Order>(0);
                var orderItems = await _baseService.Get<OrderItem>();*/
                var UserId = HttpContext.Session.GetString("UserId");
                var carts = await _baseService.Get<Order>();
                var cart = carts.FirstOrDefault(c => c.UserId == UserId);

                //var orderItems = await OrderItemService.GetAll();

                var OrderItemList = await _baseService.Get<OrderItem>();

                var OrderItems = OrderItemList.FindAll(o => o.CartId == cart.Id);

                if (cart != null)
                {
                    var CartViewModel = new OrderViewModel()
                    {
                        Order = cart,
                        OrderItems = OrderItems
                    };

                    return View(CartViewModel);

                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = $"Error : {ex}";
            }
            
            return View();
    }

        public async Task<JsonResult> CountItems()
        {
           return Json(await CountOrderItems());
        }

        public async Task<long> CountOrderItems()
        {
            //var OrderItems = await OrderItemService.GetAll();
            var Cart = await GetCartByUserId();
            var OrderItems = await _orderItemService.Get(Cart.Id);
            return OrderItems.Sum(o => o.Qty);
        }

        public async Task AddToCart(long FoodId, int Qty)
        {
            //replace by inner method
            var UserId = HttpContext.Session.GetString("UserId");

            try
            {
                var food = await _baseAPIService.Get<Food>(FoodId);
                //var cart = await _baseService.Get<Order>(0);
                var cart = await _cartService.Add(UserId);

                await _orderItemService.AddOrderItem(food, cart.Id, Qty);

                // No need to call UpdateCart here
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<Order> GetCartByUserId()
        {
            var UserId = HttpContext.Session.GetString("UserId");
            return await _cartService.Add(UserId);
        }
       /* public async Task AddToCart(long FoodId, int Qty)
        {
            /* if (HttpContext.Session.GetString("JWToken") is null || HttpContext.Session.GetString("JWToken") == "")
             {
                 return new JsonResult(new { error = "Unauthorized" }) { StatusCode = 401 };
             }
             else
             {*/

            
            //var cart = _context.Carts.FirstOrDefault();
            /*try
            {

                //var food = await FoodService.Get(FoodId);
                var food = await _baseAPIService.Get<Food>(FoodId);

                //var cart = CartService.Get();
                var cart = await _baseService.Get<Cart>(0);

                if (cart is null)
                {
                    // await CartService.Add(new Cart() { ItemsPrice = 0, TaxPrice = 0, ShippingPrice = 0, TotalPrice = 0 });
                    
                    await _baseService.Add<Cart>(new Cart() { ItemsPrice = 0, TaxPrice = 0, ShippingPrice = 0, TotalPrice = 0 });

                    //var cartResult = CartService.Get() ?? new Cart();
                    var cartResult = await _baseService.Get<Cart>(0) ?? new Cart();

                    //await OrderItemService.AddOrderItem(food, cartResult.Id, Qty);
                    await _orderItemService.AddOrderItem(food, cartResult.Id, Qty);
                }
                else
                {
                    //await OrderItemService.AddOrderItem(food, cart.Id, Qty);
                    await _orderItemService.AddOrderItem(food, cart.Id, Qty);
                }

                //await UpdateCart();
            }
            catch(Exception ex)
            {
                // TempData["Error"] = ex.ToString();
                throw;
            }            

            
        }*/

        public async Task<Order> UpdateCartPrices()
        {
            // var cart = await _context.Carts.FirstOrDefaultAsync();
            try
            {
                // var cart = CartService.Get();
                var UserId = HttpContext.Session.GetString("UserId");
                var cart = await _cartService.Get(UserId);

                //var OrderItems = await OrderItemService.GetAll();
                var OrderItems = await _orderItemService.Get(cart.Id);

                decimal itemsPrice = 0;
                decimal deliveryFee = 0;
                decimal totalPrice = 0;

                if (cart != null && OrderItems != null)
                {
                    foreach (var item in OrderItems)
                    {
                        itemsPrice += (decimal)item.Price * item.Qty;
                    }

                    totalPrice = itemsPrice + deliveryFee;
                    cart.ItemsPrice = itemsPrice;
                    cart.DeliveryFee = deliveryFee;
                    cart.TotalPrice = totalPrice;
                }

                //await CartService.Update(cart);
                await _baseService.Update<Order>(cart);

                return cart;
            }
            catch
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<JsonResult> UpdateItem(long Id, int Qty)
        {
            try
            {
                // var OrderItem = OrderItemService.GetByID(Id);
                var OrderItem = await _baseService.Get<OrderItem>(Id);
                OrderItem.Qty = Qty;

                //await OrderItemService.Update(OrderItem);
                await _baseService.Update<OrderItem>(OrderItem);
                var cart = await UpdateCartPrices();

                var countItems = await CountOrderItems();

                return Json(new { TotalPrice = cart.TotalPrice, countItems });
            }catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return new JsonResult(new { error = ex.Message.ToString()}) ;
            }

        }
        public async Task<JsonResult> RemoveToCart(long id)
        {
            try
            {
                //var OrderItem = OrderItemService.GetByID(id);
                var OrderItem = await _baseService.Get<OrderItem>(id);
                if (OrderItem is null)
                {
                    return new JsonResult(new { error = "Not Found" }) { StatusCode = 404 };
                }

                //await OrderItemService.Remove(OrderItem);
                await _baseService.Remove<OrderItem>(OrderItem);
                await UpdateCartPrices();

                return Json(OrderItem);
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return new JsonResult(new { error = ex.Message.ToString() });
            }

        }

        /*public async Task<IActionResult> Checkout()
        {
            using (var httpClient = new HttpClient())
            {
                //Check Bind 
               
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(new { cart = CartService.Get(),orderItems=await OrderItemService.GetAll()}), Encoding.UTF8, "application/json");;

                try
                {
                    using (var response = await httpClient.PostAsync("https://localhost:7110/api/order/", stringContent))
                    {

                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                          //  Console.WriteLine(result);
                            
                        }
                        else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            throw new Exception("User Unauthorized!");
                        }
                        else if (response.StatusCode == HttpStatusCode.Conflict)
                        {
                            throw new Exception($"Error: {response.StatusCode.ToString()} - User already exists.");
                        }
                        else
                        {
                            throw new Exception($"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}");
                        }


                    }
                }
                catch (HttpRequestException ex)
                {
                    throw;
                }
            }
            return Redirect("~/Cart/Index");
        }*/
    }
}
