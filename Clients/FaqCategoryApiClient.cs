using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YourNamespace.Models;

namespace YourNamespace.ApiClients
{
    public class FaqCategoryApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://your-api-base-url.com"; // Replace with your actual base URL

        public FaqCategoryApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<ApiResponse<FaqCategoryDto>> AddFaqCategoryAsync(FaqCategoryDto faqCategory)
        {
            var json = JsonConvert.SerializeObject(faqCategory);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/v1/faqs/category", content);
            return await ProcessResponseAsync<FaqCategoryDto>(response);
        }

        public async Task<ApiResponse<FaqCategoryDto>> EditFaqCategoryAsync(FaqCategoryDto faqCategory)
        {
            var json = JsonConvert.SerializeObject(faqCategory);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("/api/v1/faqs/category", content);
            return await ProcessResponseAsync<FaqCategoryDto>(response);
        }

        private async Task<ApiResponse<T>> ProcessResponseAsync<T>(HttpResponseMessage response)
        {
            var apiResponse = new ApiResponse<T>
            {
                StatusCode = (int)response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                apiResponse.Data = JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                apiResponse.Error = JsonConvert.DeserializeObject<ContentResult>(content);
            }

            return apiResponse;
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