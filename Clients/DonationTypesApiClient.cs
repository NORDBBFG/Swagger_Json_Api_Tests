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
        private const string BaseUrl = "https://example.com/api/v1";

        public DonationTypesApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<DonationTypeDto>> GetDonationTypesAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/donation/types");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<DonationTypeDto>>();
        }

        public async Task<ContentResult> AddDonationTypeAsync(DonationTypeDto donationType)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/donation/types", donationType);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ContentResult>();
        }

        public async Task<ContentResult> EditDonationTypeAsync(DonationTypeDto donationType)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/donation/types", donationType);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ContentResult>();
        }
    }

    public class ContentResult
    {
        public string Content { get; set; }
        public string ContentType { get; set; }
        public int? StatusCode { get; set; }
    }
}