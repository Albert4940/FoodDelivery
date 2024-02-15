using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using System.Text.Json;

namespace FoodDeliveryWebApp.Services
{
    public static class BaseAPIService
    {
        private static IHttpClientFactory _httpClientFactory;
        private static HttpClient _httpClient;
        //rest api uri
        public static void InitailizeHttp(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("FoodAPI");
        }

        public static async Task<T> Get<T>(long Id) where T : class
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{typeof(T).Name.ToLower()}/{Id.ToString()}");

            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(contentStream);
            }
            else
                throw new Exception($"{response.StatusCode.ToString()} - {response.ReasonPhrase}");
        }

    }
}
