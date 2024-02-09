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

        public OrderController(FoodDeliveryWebAppDbContext context)
        {
            FoodService.InitailizeHttp();
            CartService.InintializeContextDb(context);
            OrderItemService.InintializeContextDb(context);
            ShippingAddressService.InintializeContextDb(context);
        }

        // GET: OrderController
        public ActionResult Index(long OrderId = 0)
        {

            //TempData["Result"] = OrderId.ToString();
            HttpClient client = new HttpClient();
            var token = HttpContext.Session.GetString("JWToken");
            

            // Add token to request headers
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            try
            {
                using (var response = client.GetAsync("https://localhost:7110/api/order/" + OrderId).Result)
                {


                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;

                        var CartViewModel = JsonConvert.DeserializeObject<CartViewModel>(data);
                        return View(CartViewModel);
                        TempData["Result"] = $"{CartViewModel.cart.Id} - {CartViewModel.OrderItems.Count}";

                    }
                    else
                    {
                        //Add throw Exception
                        //throw new Exception($"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}");
                        TempData["Error"] = $"Error INDEX: {response.StatusCode.ToString()} - {response.ReasonPhrase}";

                    }
                }

            }
            catch (HttpRequestException ex)
            {
                TempData["error"] = $"Error Index: {ex.Message}";
                //Add throw Exception
                //throw;
                //Redirect("~Menu/Index");
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
        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
            /*try
            {
                await ShippingAddressService.Add(Order.ShippingAddress);
            }catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return View();
            }*/

            using (var httpClient = new HttpClient())
            {
                // Get token from session
                var token = HttpContext.Session.GetString("JWToken");
                var CartOrder = CartService.Get();
                var ShippingAddress = ShippingAddressService.Get();
                
                CartOrder.UserId = "string";
                ShippingAddress.Id = 0;
                ShippingAddress.UserId = "string";

                // Add token to request headers
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                //Check Bind 
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(new { 
                    Order = CartOrder, 
                    OrderItems = await OrderItemService.GetAll(), 
                    ShippingAddress
                }), Encoding.UTF8, "application/json");

                //TempData["Result"] = stringContent;

                try
                {
                    using (var response = await httpClient.PostAsync("https://localhost:7110/api/order/", stringContent))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
<<<<<<< HEAD
                            var OrderResult = JsonConvert.DeserializeObject<Cart>(result);

                            return RedirectToAction("Index", new { OrderId = OrderResult.Id});
=======
                             var OrderResult = JsonConvert.DeserializeObject<Cart>(result);
                            //TempData["Result"] = OrderResult.Id.ToString();
                            //return View();
                            //var OrderResult = JsonConvert.DeserializeObject<CartViewModel>(result);
                            return RedirectToAction("Index", new { OrderId = OrderResult.Id });
>>>>>>> FDA-51369174
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
                            // throw new Exception($"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}");
                            TempData["Error"] = $"Error Create method: {response.ToString()} - {response.ReasonPhrase}";
                            return View();
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    TempData["Error"] = ex.Message.ToString();
                    return View();
                }
            }
          //  TempData["Result"] = Order.ShippingAddress.Address.ToString() + Order.ShippingAddress.City.ToString() + Order.ShippingAddress.Country.ToString();
            return RedirectToAction(nameof(Index));
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
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

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
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
