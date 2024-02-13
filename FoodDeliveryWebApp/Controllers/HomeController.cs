using FoodDeliveryWebApp.Models;
using FoodDeliveryWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FoodDeliveryWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Uri baseAdd = new Uri("https://localhost:7110/api");
        private readonly HttpClient _client;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = new HttpClient();
            _client.BaseAddress = baseAdd;

            FoodService.InitailizeHttp(httpClientFactory);
            CategoryService.InitailizeHttp(httpClientFactory);
        }

        public async Task<IActionResult> Index()
        {           
            try
            {
                /*User user = GetCurrentUser();

                if (user != null)
                {
                    TempData["UserName"] = user.UserName;

                    HttpContext.Session.SetString("UserName", user.UserName);
                    return View(user);
                }*/
                // var Foods = GetAllFoods();
                var Foods = await FoodService.Get();
                // return foods is null ? View() : View(foods);
                //var Categories = new List<Category> { new Category { Id = 1, Title = "Fruit" }, new Category { Id = 2, Title = "Poulet" } };
                var Categories = await CategoryService.Get();

                var ShowCaseFoods = Foods.GetRange(0, 3);

                return Foods is not null ? View(new MenuViewModel { Foods = Foods, Categories= Categories, ShowCaseFoods = ShowCaseFoods }) : View();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a way that makes sense for your application
                // For now, we'll just pass an error message to the view
                TempData["error"] = $"Error: {ex.Message}";
                return View();
            }
        }

        public User GetCurrentUser()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");


            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = client.GetAsync(_client.BaseAddress + "/user/admin").Result;
            User user = null;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<User>(data);
            }
            return user;
            /*string jsonStr = await client.GetStringAsync(url);
            var res = JsonConvert.DeserializeObject<string>(jsonStr);
            ViewData["res"] = accessToken;
            return Redirect("~/Dashboard/Index");*/
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
