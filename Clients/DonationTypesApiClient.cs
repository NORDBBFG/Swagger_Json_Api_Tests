using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DonorApp.Services.DTO;

namespace DonorApp.ApiClient
{
    public class DonationTypesApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public DonationTypesApiClient(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        public async Task<ApiResponse<ContentResult>> AddDonationTypeAsync(DonationTypeDto donationTypeDto)
        {
            var url = $"{_baseUrl}/api/v1/donation/types";
            var content = new StringContent(JsonConvert.SerializeObject(donationTypeDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return new ApiResponse<ContentResult>
            {
                StatusCode = (int)response.StatusCode,
                Data = JsonConvert.DeserializeObject<ContentResult>(responseContent)
            };
        }
    }

    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
    }
}