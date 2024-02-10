using FoodDeliveryWebApp.Models;

using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FoodDeliveryWebApp.Services
{
    public static class FoodService
    {
        private static IHttpClientFactory _httpClientFactory;
        private static HttpClient _httpClient;
        public static void InitailizeHttp(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("FoodAPI");
        }

        public static async Task<Food> Get(long Id)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"food/{Id.ToString()}");
            
            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Food>(contentStream);               
            }
            else
                throw new Exception($"{response.StatusCode.ToString()} - {response.ReasonPhrase}");

            /*Food food = null;
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
            return food;*/
        }
    }
}
