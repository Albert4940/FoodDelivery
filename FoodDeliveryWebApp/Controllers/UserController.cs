using FoodDeliveryWebApp.Models;
using FoodDeliveryWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace FoodDeliveryWebApp.Controllers
{

    public class UserController : Controller
    {
        public IActionResult Index(string UserName = null, string Password = null)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                User user = new User { UserName = UserName, Password = Password };
                return View(user);
            }
              

            return View();
        }

        /*public async Task<IActionResult> LoginUser(User user)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:7110/api/user/auth", stringContent))
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
        [HttpPost]
        public async Task<IActionResult> LoginUser([Bind("Id,UserName,Password")] User user)
        {
            try
            {
                var token = await UserService.Auth(user);
                HttpContext.Session.SetString("JWToken", token);
                HttpContext.Session.SetString("UserName", user.UserName);
                return Redirect("~/Home/Index");
            }catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction("Index", new { UserName = user.UserName, Password = user.Password });
            }
            /*using (var httpClient = new HttpClient())
            {
                user.Id = "01";
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                try
                {
                    using (var response = await httpClient.PostAsync("https://localhost:7110/api/user/auth", stringContent))
                    {

                        if (response.IsSuccessStatusCode)
                        {

                            string token = await response.Content.ReadAsStringAsync();
                            HttpContext.Session.SetString("JWToken", token);
                            HttpContext.Session.SetString("UserName", user.UserName);
                            //HttpContext.Session.SetString("JWToken", new(){UserId:});
                            // TempData["Message"] = token;
                            ViewData["UserName"] = user.UserName;
                            //TempData["Token"] = HttpContext.Session.GetString("JWToken");
                            return Redirect("~/Home/Index");
                        }
                        else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {

                            TempData["Error"] = "Incorrect UserId or Password!";
                        }
                        else if(response.StatusCode == HttpStatusCode.NotFound)
                        {
                            TempData["Error"] = "Incorrect UserId or Password!";
                            return RedirectToAction("Index", new { UserName = user.UserName, Password = user.Password });
                        }
                        else
                        {
                            TempData["Error"] = $"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}";
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
            }*/
        }

        public async Task<IActionResult> LogOut()
        {

            HttpContext.Session.Clear();//
            return Redirect("~/Home/Index");
            ;
        }


        public async Task<IActionResult> Register()
        {

            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,UserName,Password")] User user)
        {
            //If USer existe 

            /* if (ModelState.IsValid)
             {

                 return RedirectToAction(nameof(Index));
             }*/
            string error = "";
            if (user.UserName != null && user.Password != null)
            {
                using (var httpClient = new HttpClient())
                {
                    user.Id = "01";

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                    try
                    {
                        using (var response = await httpClient.PostAsync("https://localhost:7110/api/user/", stringContent))
                        {

                            if (response.IsSuccessStatusCode)
                            {

                                string result = await response.Content.ReadAsStringAsync();

                                // HttpContext.Session.SetString("JWToken", token);
                                TempData["Result"] = result;
                            }
                            else if (response.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                //Change message

                                error = "Incorrect UserId or Password!";
                            }
                            else if (response.StatusCode == HttpStatusCode.Conflict)
                            {
                                error = $"Error: {response.StatusCode.ToString()} - User already exists.";
                            }
                            else
                            {
                                error = $"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}";
                            }


                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        // Handle any exceptions related to the HTTP request (e.g., network issues)
                        error = $"Error: {ex.Message}";
                    }
                }
                // return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = error;
            return View(user);
        }
    }
}