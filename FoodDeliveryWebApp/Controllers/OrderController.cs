﻿using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using FoodDeliveryWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace FoodDeliveryWebApp.Controllers
{
    public class OrderController : Controller
    {
        public OrderController(FoodDeliveryWebAppDbContext context)
        {
            FoodService.InitailizeHttp();
            CartService.InintializeContextDb(context);
            OrderItemService.InintializeContextDb(context);
        }
        // GET: OrderController
        public ActionResult Index()
        {
            return View();
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CartViewModel Order)
        {
            using (var httpClient = new HttpClient())
            {
                // Get token from session
                var token = HttpContext.Session.GetString("JWToken");
                var CartOrder = CartService.Get();
                CartOrder.UserId = "string";

                Order.ShippingAddress.Id = 0;
                Order.ShippingAddress.UserId = "string";

                // Add token to request headers
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                //Check Bind 
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(new { Order = CartOrder, OrderItems = await OrderItemService.GetAll(), ShippingAddress  = Order.ShippingAddress }), Encoding.UTF8, "application/json");

                //TempData["Result"] = stringContent;

                try
                {
                    using (var response = await httpClient.PostAsync("https://localhost:7110/api/order/", stringContent))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            TempData["Result"] = result.ToString();
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
                            TempData["Error"] = $"Error: {response.ToString()} - {response.ReasonPhrase}";
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    TempData["Error"] = ex.Message.ToString();
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
