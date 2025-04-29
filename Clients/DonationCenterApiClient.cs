// DonationCenterApiClient.cs
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DonorApp.Services.DTO;

namespace DonorApp.ApiClient
{
    public class DonationCenterApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

        public DonationCenterApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<ApiResponse<DonationCenterDto>> AddDonationCenterAsync(DonationCenterDto donationCenter)
        {
            var json = JsonConvert.SerializeObject(donationCenter);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/v1/donationCenters", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var createdDonationCenter = JsonConvert.DeserializeObject<DonationCenterDto>(responseContent);
                return new ApiResponse<DonationCenterDto>
                {
                    IsSuccess = true,
                    Data = createdDonationCenter,
                    StatusCode = (int)response.StatusCode
                };
            }
            else
            {
                var errorContent = JsonConvert.DeserializeObject<ContentResult>(responseContent);
                return new ApiResponse<DonationCenterDto>
                {
                    IsSuccess = false,
                    ErrorMessage = errorContent?.Content,
                    StatusCode = (int)response.StatusCode
                };
            }
        }
    }

    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }
}