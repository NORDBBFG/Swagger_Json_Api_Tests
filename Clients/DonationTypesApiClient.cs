using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DonorApp.Services.DTO;
using Newtonsoft.Json;

namespace DonorApp.ApiClient
{
    public class DonationTypesApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

        public DonationTypesApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<ApiResponse<List<DonationTypeDto>>> GetDonationTypesAsync()
        {
            var response = await _httpClient.GetAsync("/api/v1/donation/types");
            return await CreateApiResponseAsync<List<DonationTypeDto>>(response);
        }

        public async Task<ApiResponse<ContentResult>> AddDonationTypeAsync(DonationTypeDto donationType)
        {
            var content = new StringContent(JsonConvert.SerializeObject(donationType), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/v1/donation/types", content);
            return await CreateApiResponseAsync<ContentResult>(response);
        }

        public async Task<ApiResponse<ContentResult>> EditDonationTypeAsync(DonationTypeDto donationType)
        {
            var content = new StringContent(JsonConvert.SerializeObject(donationType), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/api/v1/donation/types", content);
            return await CreateApiResponseAsync<ContentResult>(response);
        }

        private async Task<ApiResponse<T>> CreateApiResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = new ApiResponse<T>
            {
                StatusCode = (int)response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                result.Data = JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                result.Error = JsonConvert.DeserializeObject<ContentResult>(content);
            }

            return result;
        }
    }

    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public T Data { get; set; }
        public ContentResult Error { get; set; }
    }
}