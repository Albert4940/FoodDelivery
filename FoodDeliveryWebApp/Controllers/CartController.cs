using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace FoodDeliveryWebApp.Controllers
{
    public class CartController : Controller
    {
        Uri baseAdd = new Uri("https://localhost:7110/api");
        private readonly HttpClient _client;
        private readonly FoodDeliveryWebAppDbContext _context;
        public CartController(FoodDeliveryWebAppDbContext context)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAdd;
            _context = context; 
        }
        // GET: CartController
        public ActionResult Index()
        {
            var OrderItem = _context.OrderItems.FirstOrDefault() ?? new OrderItem();
            ViewData["Title"] = OrderItem.Title;
            //var cart = string.IsNullOrEmpty(cartData) ? new Cart() : JsonConvert.DeserializeObject<Cart>(cartData);

            /*Cart cartView = new Cart()
            {
                
                PaymentMethod = cart.PaymentMethod
            };*/
            return View();
        }

        public async Task AddOrderItem(Food food, long cartId)
        {
            var OrderItem = new OrderItem() { Title = food.Title, CartId = cartId, Qty = 0, ImageURL = "", Price = 0, ProductId = food.Id };
            try
            {


                _context.Add(OrderItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.ToString();

            }
        }

        // [SessionAuthorize]
        //[HttpPost]
        public async Task<JsonResult> AddToCart(long id)
        {
           /* if (HttpContext.Session.GetString("JWToken") is null || HttpContext.Session.GetString("JWToken") == "")
            {
                return new JsonResult(new { error = "Unauthorized" }) { StatusCode = 401 };
            }
            else
            {*/
                var food = Get(id);
            var cart = new Cart() { ItemsPrice = 0, TaxPrice = 0, ShippingPrice = 0, TotalPrice = 0};

               
            try
            {
                _context.Add(cart);
                 await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.ToString();
            }

                var cartResult = _context.Carts.FirstOrDefault() ?? new Cart();
            await AddOrderItem(food, cartResult.Id);
                
                return Json(food);
            //}

            /* var successData = new { message = "Item added to cart successfully" };

             return Json(new Food () { Id = id, Title = "Poulet" });*/

        }

        public Food Get(long id)
        {

            Food food = null;
            HttpClient client = new HttpClient();
            try
            {
                using (var response = client.GetAsync(_client.BaseAddress + "/food/" + id).Result)
                {


                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        food = JsonConvert.DeserializeObject<Food>(data);
                    }
                    else
                    {
                        TempData["Error"] = $"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}";
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                TempData["error"] = $"Error: {ex.Message}";
                //Add throw Exception
                Redirect("~Menu/Index");
            }

            // HttpResponseMessage response = client.GetAsync(_client.BaseAddress + "/food").Result;


            return food;
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
