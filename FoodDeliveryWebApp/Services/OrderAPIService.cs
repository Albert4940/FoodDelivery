using FoodDeliveryWebApp.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static FoodDeliveryWebApp.Services.OrderAPIService;

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
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception(errorContent.ToString());
            }
        }

        public async Task<List<T>> Get<T>(string token = null) where T : class
        {
            if (string.IsNullOrWhiteSpace(token) || token == "")
                throw new ArgumentOutOfRangeException(nameof(token), "The token cannot be null or empty.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //string endPoint = typeof(T).Name.ToLower();

            using HttpResponseMessage response = await _httpClient.GetAsync($"{endPoint}/");

            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<T>>(contentStream);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception(errorContent.ToString());
            }                
        }

        public async Task<Order> Add(OrderViewModel model, string token)
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
                return await JsonSerializer.DeserializeAsync<Order>(contentStream);
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception(errorContent.ToString());
                /*var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent);
                throw new Exception(errorResponse.ErrorMessage);*/
            }
            else
            {
                //throw new Exception($"{response.StatusCode.ToString()} - {response.ReasonPhrase}");
                string errorMessage = $"{response.StatusCode.ToString()} - {response.ReasonPhrase}";
                throw new HttpRequestException(errorMessage);
            }
        }

        public class ErrorResponse
        {
            public string ErrorMessage { get; set; }
        }
    }
}
