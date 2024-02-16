﻿using FoodDeliveryWebApp.Models;
using System.Text;
using System.Text.Json;

namespace FoodDeliveryWebApp.Services
{
    public class UserAPIService : BaseAPIService
    {
        private string endPoint = "user";

        public UserAPIService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public  async Task<string> Auth(User user)
        {
            user.Id = "01";
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await _httpClient.PostAsync($"{endPoint}/auth/", jsonContent);

            if (response.IsSuccessStatusCode)
            {
               return await response.Content.ReadAsStringAsync();
                //return await JsonSerializer.DeserializeAsync<T>(contentStream);
                //return item;
            }
            else
                throw new Exception($"{response.StatusCode.ToString()} - {response.ReasonPhrase}");
        }
    }
}
