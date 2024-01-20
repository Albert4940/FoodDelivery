﻿using FoodDeliveryWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FoodDeliveryWebApp.Controllers
{
    public class FoodController : Controller
    {
        Uri baseAdd = new Uri("https://localhost:7110/api");
        private readonly HttpClient _client;

        public FoodController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAdd;
        }

        // GET: FoodController
        public ActionResult Index(long id)
        {
            var food = GetFood(id);
            food.CountInStock = 7;
           return food is null ?  View() :  View(food);
        }

        public Food GetFood(long id)
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
               // Redirect("~Menu/Index");
            }

            // HttpResponseMessage response = client.GetAsync(_client.BaseAddress + "/food").Result;


            return food;
        }
        

        // GET: FoodController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FoodController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FoodController/Create
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

        // GET: FoodController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FoodController/Edit/5
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

        // GET: FoodController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FoodController/Delete/5
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