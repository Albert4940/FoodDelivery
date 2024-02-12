using FoodDeliveryWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace FoodDeliveryWebApp.Controllers
{
    public class MenuController : Controller
    {
        Uri baseAdd = new Uri("https://localhost:7110/api");
        private readonly HttpClient _client;

        public MenuController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAdd;
        }
        // GET: MenuController
        public async Task<ActionResult> Index()
        {


            try
            {
                var foods = await GetAllFoods();
                return foods is null ? View() : View(foods);
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