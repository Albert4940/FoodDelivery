using FoodDeliveryWebApp.Models;
using FoodDeliveryWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FoodDeliveryWebApp.Controllers
{

    public class UserController : Controller
    {
        private readonly UserAPIService _userAPIService;
        private readonly BaseAPIService _baseAPIService;
        public UserController(IHttpClientFactory httpClientFactory)
        {
            _userAPIService = new UserAPIService(httpClientFactory);
            _baseAPIService = new BaseAPIService(httpClientFactory);
        }

        public IActionResult Index(string UserName = null, string Password = null)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                User user = new User { UserName = UserName, Password = Password };
                return View(user);
            }
              
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LoginUser([Bind("Id,UserName,Password")] User user)
        {
            try
            {
                //var token = await UserService.Auth(user);
                var userAuth = await _userAPIService.Auth(user);

                HttpContext.Session.SetString("JWToken", userAuth.Token);
                HttpContext.Session.SetString("UserId", userAuth.Id);
                HttpContext.Session.SetString("UserName", user.UserName);

                return Redirect("~/Home/Index");
            }catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction("Index", new { UserName = user.UserName, Password = user.Password });
            }
        }

        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Clear();//
            return Redirect("~/Home/Index");
        }


        public async Task<IActionResult> Register()
        {
           return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,UserName,Password")] User user)
        {
            try
            {
                //var data = await UserService.Register(user);
                //Check Bind 
                user.Id = "01";

                var data = await _userAPIService.Add<User>(user);

                TempData["Result"] = "User Register successfully!";

                return Redirect("/User/Index");
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
            }           
            return View(user);
        }

        [SessionExpire]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var UserId = HttpContext.Session.GetString("UserId");
            var UserName = HttpContext.Session.GetString("UserName");

            try
            {
                var Addresses = await _baseAPIService.Get<ShippingAddress>();
                var ShippingAddress = Addresses.FirstOrDefault(a => a.UserId == UserId);

                var UserInfo = new OrderViewModel
                {
                    User = new User { UserName = UserName },
                    ShippingAddress = ShippingAddress
                };

                return View(UserInfo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Profile(User user, string confirmPassword)
        {
            if(user.Password != confirmPassword)
            {
                TempData["Error"] = "The two passwords must match!";
                return View();
            }

            return View();
        }


    }
}