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
        //rest api uri
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
        }

        public static async Task<List<Food>> Get()
        {
            using HttpResponseMessage response = await _httpClient.GetAsync("food/");

            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<Food>>(contentStream);
            }else
                throw new Exception($"{response.StatusCode.ToString()} - {response.ReasonPhrase}");

        }
    }
}
