using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DonorApp.Services.DTO;

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

        public async Task<List<DonationTypeDto>> GetDonationTypesAsync()
        {
            var response = await _httpClient.GetAsync("/api/v1/donation/types");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<DonationTypeDto>>();
        }

        public async Task<HttpResponseMessage> AddDonationTypeAsync(DonationTypeDto donationType)
        {
            return await _httpClient.PostAsJsonAsync("/api/v1/donation/types", donationType);
        }

        public async Task<HttpResponseMessage> EditDonationTypeAsync(DonationTypeDto donationType)
        {
            return await _httpClient.PutAsJsonAsync("/api/v1/donation/types", donationType);
        }
    }
}
