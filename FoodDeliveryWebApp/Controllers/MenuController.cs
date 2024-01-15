using FoodDeliveryWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace FoodDeliveryWebApp.Controllers
{
    public class MenuController : Controller
    {
        Uri baseAdd = new Uri("https://localhost:44339/api");
        private readonly HttpClient _client;

        public MenuController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAdd;
        }
        // GET: MenuController
        public async Task<ActionResult> Index()
        {
            var foods = await GetAllFoods();

            return View(foods);
        }

        public async Task<List<Food>> GetAllFoods()
        {
            HttpClient client = new HttpClient();       
             HttpResponseMessage response = client.GetAsync(_client.BaseAddress + "/food").Result;
             List<Food> foods = null;

             if (response.IsSuccessStatusCode)
             {
                 string data = response.Content.ReadAsStringAsync().Result;
                 foods = JsonConvert.DeserializeObject<List<Food>>(data);
             }

             return foods;
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