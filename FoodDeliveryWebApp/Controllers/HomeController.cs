using FoodDeliveryWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace FoodDeliveryWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Uri baseAdd = new Uri("https://localhost:44339/api");
        private readonly HttpClient _client;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _client = new HttpClient();
            _client.BaseAddress = baseAdd;
        }

        public IActionResult Index()
        {
            try
            {
                User user = GetCurrentUser();

                if (user != null)
                {
                    TempData["UserName"] = user.UserName;

                    HttpContext.Session.SetString("UserName", user.UserName);
                    return View(user);
                }
                return View();
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
