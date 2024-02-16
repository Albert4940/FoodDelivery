using FoodDeliveryWebApp.Models;
using FoodDeliveryWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;


namespace FoodDeliveryWebApp.Controllers
{
    public class MenuController : Controller
    {
        Uri baseAdd = new Uri("https://localhost:7110/api");
        private readonly HttpClient _client;

        public MenuController(IHttpClientFactory httpClientFactory)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAdd;

           // BaseAPIService.InitailizeHttp(httpClientFactory);
            FoodService.InitailizeHttp(httpClientFactory);
            CategoryService.InitailizeHttp(httpClientFactory);
        }
        // GET: MenuController
        public async Task<ActionResult> Index()
        {


            try
            {
                var Foods = await FoodService.Get();
                 var Categories = await CategoryService.Get();

               // var Foods = await BaseAPIService.Get<Food>();
                //var Categories = await BaseAPIService.Get<Category>();

                return Foods is not null ? View(new MenuViewModel { Foods = Foods, Categories = Categories }) : View();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a way that makes sense for your application
                // For now, we'll just pass an error message to the view
                TempData["error"] = $"Error: {ex.Message}";
                return View();
            }
        }

        public async Task<List<Food>> GetAllFoods()
        {
            List<Food> foods = null;
            HttpClient client = new HttpClient();
            try
            {
                using(var response = client.GetAsync(_client.BaseAddress + "/food").Result)
                {
                   

                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        foods = JsonConvert.DeserializeObject<List<Food>>(data);
                    }
                    else
                    {
                        TempData["Error"] = $"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}";
                    }
                }

            }catch(HttpRequestException ex)
            {
                TempData["error"] = $"Error: {ex.Message}";
                Redirect("~Menu/Index");
            }
                 
            // HttpResponseMessage response = client.GetAsync(_client.BaseAddress + "/food").Result;
            

             return foods;
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

        // GET: MenuController/Details/5
        /*public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MenuController/Create
        public  ActionResult Create()
        {
    
            return View();
        }



        // POST: MenuController/Create
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

        // GET: MenuController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MenuController/Edit/5
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

        // GET: MenuController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MenuController/Delete/5
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
        }*/
    }
}