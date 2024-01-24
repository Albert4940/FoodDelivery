using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using FoodDeliveryWebApp.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

namespace FoodDeliveryWebApp.Controllers
{
    public class CartController : Controller
    {
        //Uri baseAdd = new Uri("https://localhost:7110/api");
        //private readonly HttpClient _client;
        private readonly FoodDeliveryWebAppDbContext _context;
        public CartController(FoodDeliveryWebAppDbContext context)
        {
            /* _client = new HttpClient();
             _client.BaseAddress = baseAdd;*/
            FoodService.InitailizeHttp();
            CartService.InintializeContextDb(context);
            OrderItemService.InintializeContextDb(context);

            _context = context; 
        }
        //Get
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var cart = CartService.Get();
                var orderItems = _context.OrderItems.ToList();

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
        public ActionResult Index(long Id, int CountInStock)
        {
            long FoodId = Id;
            int Qty = CountInStock;

            //
            try
            {
                AddToCart(FoodId, Qty);
            }catch(Exception ex)
            {
                //if err redirect to Food Page
                TempData["Error"] = $"Error : {ex}";
            }
            // TempData["Data"] = $"Data: {Id} - {CountInStock}";
            //var cart = _context.Carts.FirstOrDefault();
            var cart = CartService.Get();
            var orderItems = _context.OrderItems.ToList();

             if(cart != null)
             {
                 var CartViewModel = new CartViewModel()
                 {
                     cart = cart,
                     OrderItems = orderItems
                 };

                

                 return View(CartViewModel);
              
            }
            //ViewData["Title"] = OrderItem.Title;
            //var cart = string.IsNullOrEmpty(cartData) ? new Cart() : JsonConvert.DeserializeObject<Cart>(cartData);

            /*Cart cartView = new Cart()
            {
                
                PaymentMethod = cart.PaymentMethod
            };*/
            //return View();
               
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
        public async Task AddOrderItem(Food food, long cartId, int qty)
        {
            //var OrderItem =
           
            var OrderItem = _context.OrderItems.FirstOrDefault(o => o.ProductId == food.Id && o.CartId == cartId);

            if(OrderItem is null)
            {
                try
                {
                    _context.Add(new OrderItem() { 
                        Title = food.Title, CartId = cartId, 
                        Qty = qty, ImageURL = food.ImageURL, 
                        Price = food.Price, ProductId = food.Id,
                        CountInStock = food.CountInStock
                    });
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //re-trhow
                    TempData["Error"] = ex.ToString();

                }
            }
            else
            {
                OrderItem.Qty = qty;
                OrderItem.CountInStock = food.CountInStock;

                _context.Entry(OrderItem).State = EntityState.Modified;
                _context.Update(OrderItem);
                await _context.SaveChangesAsync(); 
            }

        }

        // [SessionAuthorize]
        //[HttpPost]
        /*public async Task<JsonResult> AddToCart(long id)
        {*/
           /* if (HttpContext.Session.GetString("JWToken") is null || HttpContext.Session.GetString("JWToken") == "")
            {
                return new JsonResult(new { error = "Unauthorized" }) { StatusCode = 401 };
            }
            else
            {*/
                
           /* var food = Get(id);
            var cart = _context.Carts.FirstOrDefault() ;
            if(cart is null)
            {
                try
                {
                    _context.Add(new Cart() { ItemsPrice = 0, TaxPrice = 0, ShippingPrice = 0, TotalPrice = 0 });
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.ToString();
                }

                var cartResult = _context.Carts.FirstOrDefault() ?? new Cart();
                await AddOrderItem(food, cartResult.Id);
            }
            else
            {
                await AddOrderItem(food, cart.Id);
            }
               

                
                return Json(food);*/


            //}

            /* var successData = new { message = "Item added to cart successfully" };

             return Json(new Food () { Id = id, Title = "Poulet" });*/

       // }

        public async Task AddToCart(long FoodId, int Qty)
        {
            /* if (HttpContext.Session.GetString("JWToken") is null || HttpContext.Session.GetString("JWToken") == "")
             {
                 return new JsonResult(new { error = "Unauthorized" }) { StatusCode = 401 };
             }
             else
             {*/

            var food = FoodService.Get(FoodId);

            var cart = CartService.Get();
            //var cart = _context.Carts.FirstOrDefault();
            if (cart is null)
            {
                try
                {
                    _context.Add(new Cart() { ItemsPrice = 0, TaxPrice = 0, ShippingPrice = 0, TotalPrice = 0 });
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.ToString();
                }

                var cartResult = CartService.Get() ?? new Cart();
                await AddOrderItem(food, cartResult.Id, Qty);
            }
            else
            {
                await AddOrderItem(food, cart.Id, Qty);
            }



            //return Json(food);


            //}

            /* var successData = new { message = "Item added to cart successfully" };

             return Json(new Food () { Id = id, Title = "Poulet" });*/
           
            await UpdateCart();
        }

        public async Task<Cart> UpdateCart()
        {
            // var cart = await _context.Carts.FirstOrDefaultAsync();
            var cart = CartService.Get();
            var OrderItems = await _context.OrderItems.ToListAsync();

            double itemsPrice = 0;
            double shippingPrice = 0;
            double totalPrice = 0;

            if(cart != null && OrderItems != null)
            {
                foreach(var item in OrderItems)
                {
                    itemsPrice +=  item.Price * item.Qty; 
                }
                totalPrice = itemsPrice + shippingPrice;
                cart.ItemsPrice = itemsPrice;
                cart.ShippingPrice = shippingPrice;
                cart.TotalPrice = totalPrice;
            }

            _context.Entry(cart).State = EntityState.Modified;
            _context.Update(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        [HttpPost]
        public async Task<JsonResult> UpdateItem(long Id, int Qty)
        {
            var OrderItem = OrderItemService.GetByID(Id);
            OrderItem.Qty = Qty;

            await OrderItemService.Update(OrderItem);
            var cart = await UpdateCart();

            var countItems = await CountOrderItems();

            return Json(new {TotalPrice = cart.TotalPrice,countItems});
            //return Json(OrderItem);
        }
        public async Task<JsonResult> RemoveToCart(long id)
        {
            var OrderItem = await _context.OrderItems.FirstOrDefaultAsync(x => x.Id == id);
            if(OrderItem is null)
            {
                return new JsonResult(new { error = "Not Found" }) { StatusCode = 404 };
            }
             _context.OrderItems.Remove(OrderItem);
            await _context.SaveChangesAsync();


            await UpdateCart();

            return Json(OrderItem);
        }

        // GET: CartController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CartController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CartController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: CartController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CartController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CartController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
