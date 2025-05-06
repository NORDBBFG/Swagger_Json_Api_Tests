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
            try
            {
                var response = await _httpClient.GetAsync("/api/v1/donation/types");
                response.EnsureSuccessStatusCode();
                var donationTypes = await response.Content.ReadFromJsonAsync<List<DonationTypeDto>>();
                return new ApiResponse<List<DonationTypeDto>>(response.StatusCode, donationTypes);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<List<DonationTypeDto>>(System.Net.HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }
    }

    public class ApiResponse<T>
    {
        public System.Net.HttpStatusCode StatusCode { get; }
        public T Data { get; }
        public string ErrorMessage { get; }

        public ApiResponse(System.Net.HttpStatusCode statusCode, T data, string errorMessage = null)
        {
            StatusCode = statusCode;
            Data = data;
            ErrorMessage = errorMessage;
        }
    }
}
