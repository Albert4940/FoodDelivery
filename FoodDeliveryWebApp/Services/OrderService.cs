using FoodDeliveryWebApp.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FoodDeliveryWebApp.Services
{
    public static class OrderService
    {
        private static  IHttpClientFactory _httpClientFactory;
        private static HttpClient _httpClient;
        public static void InitailizeHttp(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("FoodAPI");
        }

        public static async Task<OrderViewModel> Get(long Id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await _httpClient.GetAsync($"order/{Id.ToString()}");
           
            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<OrderViewModel>(contentStream);
            }
            else
            {
               throw new Exception($"{response.StatusCode.ToString()} - {response.ReasonPhrase}");
            }
        }

        public static async Task<Order> Add(OrderViewModel model, string token)
        {
           

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(new
                {
                    Order = model.cart,
                    OrderItems = model.OrderItems,
                    ShippingAddress = model.ShippingAddress
                }),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await _httpClient.PostAsync("order/", jsonContent);
           
            //return response.ToString();
            if (response.IsSuccessStatusCode) 
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Order>(contentStream);
                //return await JsonSerializer.DeserializeAsync<string>(contentStream);
            }
            else
            {
                throw new Exception($"{response.StatusCode.ToString()} - {response.ReasonPhrase}");
            }
        }
    }
}
