using FoodDeliveryWebApp.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace FoodDeliveryWebApp.Services
{
    public static class UserService
    {
       /* static Uri baseAdd = new Uri("https://localhost:7110/api");
        private static HttpClient _client;*/

        public static void InitailizeHttp()
        {
            /*_client = new HttpClient();
            _client.BaseAddress = baseAdd;*/
        }

        public static async Task<string> Auth(User user)
        {
            string token = null;
            using (var httpClient = new HttpClient())
            {
                user.Id = "01";
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                try
                {
                    using (var response = await httpClient.PostAsync("https://localhost:7110/api/user/auth", stringContent))
                    {

                        if (response.IsSuccessStatusCode)
                        {

                             token = await response.Content.ReadAsStringAsync();
                           // HttpContext.Session.SetString("JWToken", token);
                           // HttpContext.Session.SetString("UserName", user.UserName);

                            //HttpContext.Session.SetString("JWToken", new(){UserId:});
                            // TempData["Message"] = token;
                           // ViewData["UserName"] = user.UserName;
                            //TempData["Token"] = HttpContext.Session.GetString("JWToken");
                           // return Redirect("~/Home/Index");
                        }
                        else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {

                            throw new Exception(response.ReasonPhrase.ToString());
                        }
                        else if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            throw new Exception("Incorrect UserId or Password!");
                            //return RedirectToAction("Index", new { UserName = user.UserName, Password = user.Password });
                        }
                        else
                        {
                            // TempData["Error"] = $"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}";
                            throw new Exception($"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}");
                        }

                        //return Redirect("~/Home/Index");
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle any exceptions related to the HTTP request (e.g., network issues)
                    // ViewBag.Message = $"Error: {ex.Message}";
                    // return Redirect("~/Home/Index");
                    throw;
                }
            }
            return token;
        }
       
    }
}
