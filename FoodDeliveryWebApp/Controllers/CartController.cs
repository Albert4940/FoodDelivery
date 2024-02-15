using FoodDeliveryWebApp.Data;
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

        public CartController(FoodDeliveryWebAppDbContext context, IHttpClientFactory httpClientFactory)
        {
           
            FoodService.InitailizeHttp(httpClientFactory);
            CartService.InintializeContextDb(context);
            OrderItemService.InintializeContextDb(context);
            BaseService.InintializeContextDb(context);
        }
        //Get
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                //var cart = CartService.Get();
                var cart = BaseService.Get<Cart>();
                var orderItems = await OrderItemService.GetAll();

                if (cart != null)
                {
                    var CartViewModel = new CartViewModel()
                    {
                        cart = cart,
                        OrderItems = orderItems
                    };
                    return View(CartViewModel);
                }
            }catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return View();
            }
            return View();
        }

        [HttpPost]
        // Post: CartController
        public async Task<ActionResult> Index(long Id, int CountInStock)
        {
            long FoodId = Id;
            int Qty = CountInStock;

            
            try
            {
                await AddToCart(FoodId, Qty);
            }catch(Exception ex)
            {               
                TempData["Error"] = $"Error : {ex}";
            }

            try
            {
                var cart = CartService.Get();
                var orderItems = await OrderItemService.GetAll();

                if (cart != null)
                {
                    var CartViewModel = new CartViewModel()
                    {
                        cart = cart,
                        OrderItems = orderItems
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
            var OrderItems = await OrderItemService.GetAll();
            return OrderItems.Sum(o => o.Qty);
        }
 

        public async Task AddToCart(long FoodId, int Qty)
        {
            /* if (HttpContext.Session.GetString("JWToken") is null || HttpContext.Session.GetString("JWToken") == "")
             {
                 return new JsonResult(new { error = "Unauthorized" }) { StatusCode = 401 };
             }
             else
             {*/

            
            //var cart = _context.Carts.FirstOrDefault();
            try
            {

                var food = await FoodService.Get(FoodId);

                var cart = CartService.Get();

                if (cart is null)
                {                    
                        await CartService.Add(new Cart() { ItemsPrice = 0, TaxPrice = 0, ShippingPrice = 0, TotalPrice = 0 });                       

                        var cartResult = CartService.Get() ?? new Cart();
                        await OrderItemService.AddOrderItem(food, cartResult.Id, Qty);
                }
                else
                {
                    await OrderItemService.AddOrderItem(food, cart.Id, Qty);
                }

                await UpdateCart();
            }
            catch(Exception ex)
            {
                // TempData["Error"] = ex.ToString();
                throw;
            }            

            
        }

        public async Task<Cart> UpdateCart()
        {
            // var cart = await _context.Carts.FirstOrDefaultAsync();
            try
            {
                var cart = CartService.Get();
                var OrderItems = await OrderItemService.GetAll();

                double itemsPrice = 0;
                double shippingPrice = 0;
                double totalPrice = 0;

                if (cart != null && OrderItems != null)
                {
                    foreach (var item in OrderItems)
                    {
                        itemsPrice += item.Price * item.Qty;
                    }
                    totalPrice = itemsPrice + shippingPrice;
                    cart.ItemsPrice = itemsPrice;
                    cart.ShippingPrice = shippingPrice;
                    cart.TotalPrice = totalPrice;
                }

                await CartService.Update(cart);
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
                var OrderItem = OrderItemService.GetByID(Id);
                OrderItem.Qty = Qty;

                await OrderItemService.Update(OrderItem);
                var cart = await UpdateCart();

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
                var OrderItem = OrderItemService.GetByID(id);

                if (OrderItem is null)
                {
                    return new JsonResult(new { error = "Not Found" }) { StatusCode = 404 };
                }

                await OrderItemService.Remove(OrderItem);

                await UpdateCart();

                return Json(OrderItem);
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return new JsonResult(new { error = ex.Message.ToString() });
            }

        }

        public async Task<IActionResult> Checkout()
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
        }
    }
}
