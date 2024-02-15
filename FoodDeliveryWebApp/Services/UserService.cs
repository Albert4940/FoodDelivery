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

        public static async Task<User> Register(User user)
        {
            string error = "";
            if (user.UserName != null && user.Password != null)
            {
                using (var httpClient = new HttpClient())
                {
                    //Check Bind 
                    user.Id = "01";

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                    
                    try
                    {
                        using (var response = await httpClient.PostAsync("https://localhost:7110/api/user/", stringContent))
                        {

                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                return user;
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
                                throw new Exception($"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}");
                            }


                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        throw;
                    }
                }
            }
            else
            {
               throw new Exception("Error : UserName or Password is Empty!");
            }
            return null;
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
                        }
                        else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            throw new Exception(response.ReasonPhrase.ToString());
                        }
                        else if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            throw new Exception("Incorrect User or Password!");
                        }
                        else
                        {
                            throw new Exception($"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw;
                }
            }
            return token;
        }
       
    }
}
