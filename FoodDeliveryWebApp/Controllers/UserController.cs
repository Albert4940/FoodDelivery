using FoodDeliveryWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace FoodDeliveryWebApp.Controllers
{

    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /*public async Task<IActionResult> LoginUser(User user)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:44339/api/user/auth", stringContent))
                {
                    string token = await response.Content.ReadAsStringAsync();
                    if (token == "Invalid credentials")
                    {
                        ViewBag.Message = "Incorrect UserId or Password!";
                        return Redirect("~/Home/Index");
                    }

                    HttpContext.Session.SetString("JWToken", token);
                    ViewData["res"] = token;
                     return Redirect("~/Home/Index");
                }
               
            }
        }*/
        public async Task<IActionResult> LoginUser(User user)
        {
            
            using (var httpClient = new HttpClient())
            {
                user.Id = "01";
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
               
                try
                {
                    using (var response = await httpClient.PostAsync("https://localhost:44339/api/user/auth", stringContent))
                    {
                        
                        if (response.IsSuccessStatusCode)
                        {
                            
                            string token = await response.Content.ReadAsStringAsync();
                            HttpContext.Session.SetString("JWToken", token);
                            // TempData["Message"] = token;
                            return Redirect("~/Home/Index");
                        }
                        else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {

                            TempData["Message"] = "Incorrect UserId or Password!";
                        }
                        else
                        {
                            TempData["Message"]  = $"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}";
                        }

                        return Redirect("~/Home/Index");
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle any exceptions related to the HTTP request (e.g., network issues)
                    ViewBag.Message = $"Error: {ex.Message}";
                    return Redirect("~/Home/Index");
                }
            }
        }

    }
}
