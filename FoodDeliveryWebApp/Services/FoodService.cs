using FoodDeliveryWebApp.Models;
using Newtonsoft.Json;

namespace FoodDeliveryWebApp.Services
{
    public static class FoodService
    {
        static  Uri baseAdd = new Uri("https://localhost:7110/api");
        private static  HttpClient _client;

        public static void InitailizeHttp()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAdd;
        }

        public static Food Get(long id)
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
                        //Add throw Exception
                        throw new Exception($"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}");
                        //TempData["Error"] = $"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}";
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                //TempData["error"] = $"Error: {ex.Message}";
                //Add throw Exception
                throw;
                //Redirect("~Menu/Index");
            }

            // HttpResponseMessage response = client.GetAsync(_client.BaseAddress + "/food").Result;
            return food;
        }
    }
}
