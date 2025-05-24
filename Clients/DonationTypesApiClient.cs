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

        public async Task<ApiResponse<List<DonationTypeDto>>> GetDonationTypesAsync()
        {
            var response = await _httpClient.GetAsync("/api/v1/donation/types");
            
            if (response.IsSuccessStatusCode)
            {
                var donationTypes = await response.Content.ReadFromJsonAsync<List<DonationTypeDto>>();
                return new ApiResponse<List<DonationTypeDto>>(donationTypes, (int)response.StatusCode);
            }
            else
            {
                var errorContent = await response.Content.ReadFromJsonAsync<ContentResult>();
                return new ApiResponse<List<DonationTypeDto>>(null, (int)response.StatusCode, errorContent?.Content);
            }
        }

        public async Task<ApiResponse<ContentResult>> AddDonationTypeAsync(DonationTypeDto donationType)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/donation/types", donationType);
            var content = await response.Content.ReadFromJsonAsync<ContentResult>();
            return new ApiResponse<ContentResult>(content, (int)response.StatusCode);
        }

        public async Task<ApiResponse<ContentResult>> EditDonationTypeAsync(DonationTypeDto donationType)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/v1/donation/types", donationType);
            var content = await response.Content.ReadFromJsonAsync<ContentResult>();
            return new ApiResponse<ContentResult>(content, (int)response.StatusCode);
        }
    }

    public class ApiResponse<T>
    {
        public T Data { get; }
        public int StatusCode { get; }
        public string ErrorMessage { get; }

        public ApiResponse(T data, int statusCode, string errorMessage = null)
        {
            Data = data;
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
    }
}
