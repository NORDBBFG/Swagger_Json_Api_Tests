// DonationTypesApiClient.cs
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
        private const string BaseUrl = "api/v1/donation/types";

        public DonationTypesApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<ApiResponse<List<DonationTypeDto>>> GetDonationTypesAsync()
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            return await ApiResponse<List<DonationTypeDto>>.CreateAsync(response);
        }

        public async Task<ApiResponse<ContentResult>> AddDonationTypeAsync(DonationTypeDto donationType)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, donationType);
            return await ApiResponse<ContentResult>.CreateAsync(response);
        }

        public async Task<ApiResponse<ContentResult>> EditDonationTypeAsync(DonationTypeDto donationType)
        {
            var response = await _httpClient.PutAsJsonAsync(BaseUrl, donationType);
            return await ApiResponse<ContentResult>.CreateAsync(response);
        }
    }

    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; }

        public static async Task<ApiResponse<T>> CreateAsync(HttpResponseMessage response)
        {
            var apiResponse = new ApiResponse<T>
            {
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode,
                ReasonPhrase = response.ReasonPhrase
            };

            if (response.IsSuccessStatusCode)
            {
                apiResponse.Data = await response.Content.ReadFromJsonAsync<T>();
            }

            return apiResponse;
        }
    }
}
