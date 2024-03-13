using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FoodDeliveryWebApp.Services
{
    public  class BaseAPIService
    {
        //private static IHttpClientFactory _httpClientFactory;
        //private static HttpClient _httpClient;

        protected IHttpClientFactory _httpClientFactory;
        protected HttpClient _httpClient;
        //rest api uri : FODDAPI
        /* public static void InitailizeHttp(IHttpClientFactory httpClientFactory)
         {
             _httpClientFactory = httpClientFactory;
             _httpClient = _httpClientFactory.CreateClient("FoodAPI");
         }*/
        public BaseAPIService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("FoodAPI");
        }

        public virtual async Task<T> Get<T>(long Id) where T : class
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

        public  async Task<List<T>> Get<T>() where T : class
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{typeof(T).Name.ToLower()}/");

            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<T>>(contentStream);
            }
            else
                throw new Exception($"{response.StatusCode.ToString()} - {response.ReasonPhrase}");
        }

        public async Task<T> Add<T>(T item) where T : class
        {
            var jsonContent = new StringContent(
                 JsonSerializer.Serialize(item),
                 Encoding.UTF8,
                 "application/json");

            using HttpResponseMessage response = await _httpClient.PostAsync($"{typeof(T).Name.ToLower()}/", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                //return await JsonSerializer.DeserializeAsync<T>(contentStream);
                return item;
            }
            else
                throw new Exception($"{response.StatusCode.ToString()} - {response.ReasonPhrase}");
        }

        public async Task Update<T>(long Id, T item) where T : class
        {
            var jsonContent = new StringContent(
                    JsonSerializer.Serialize(item),
                    Encoding.UTF8,
                    "application/json"
                 );

            using HttpResponseMessage response = await _httpClient.PutAsync($"{typeof(T).Name.ToLower()}/{Id.ToString()}", jsonContent);
            if(response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                //return item;
            }            
        }

        public async Task Delete<T>(long Id) where T : class
        {
            using HttpResponseMessage response = await _httpClient.DeleteAsync($"{typeof(T).Name.ToLower()}/{Id.ToString()}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.StatusCode.ToString()} - {response.ReasonPhrase}");

            }
        }

    }
}
