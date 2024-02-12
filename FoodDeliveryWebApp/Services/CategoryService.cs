using FoodDeliveryWebApp.Models;
using System.ComponentModel;
using System.Text.Json;

namespace FoodDeliveryWebApp.Services
{
    public static class CategoryService
    {
        private static IHttpClientFactory _httpClientFactory;
        private static HttpClient _httpClient;
        //rest api uri
        public static void InitailizeHttp(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("FoodAPI");
        }

        public static async Task<List<Category>> Get()
        {
            using HttpResponseMessage response = await _httpClient.GetAsync("category/");

            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<Category>>(contentStream);
            }
            else
                throw new Exception($"{response.StatusCode.ToString()} - {response.ReasonPhrase}");

        }
    }
}
