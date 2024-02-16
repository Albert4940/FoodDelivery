using FoodDeliveryWebApp.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FoodDeliveryWebApp.Services
{
    public class OrderAPIService : BaseAPIService
    {
        private string endPoint = "order";

        public OrderAPIService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public async Task<T> Get<T>(long Id, string token = null) where T : class
        {
            if(string.IsNullOrWhiteSpace(token) || token == "")
                throw new ArgumentOutOfRangeException(nameof(token), "The token cannot be null or empty.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //string endPoint = typeof(T).Name.ToLower();

            using HttpResponseMessage response = await _httpClient.GetAsync($"{endPoint}/{Id.ToString()}");

            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(contentStream);
            }
            else
                throw new Exception($"{response.StatusCode.ToString()} - {response.ReasonPhrase}");
        }

        public async Task<Cart> Add(CartViewModel model, string token)
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

            using HttpResponseMessage response = await _httpClient.PostAsync($"{endPoint}/", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Cart>(contentStream);
            }
            else
            {
                throw new Exception($"{response.StatusCode.ToString()} - {response.ReasonPhrase}");
            }
        }
    }
}
