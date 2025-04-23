using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YourNamespace.Models;

namespace YourNamespace.ApiClients
{
    public class FaqApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://your-api-base-url.com"; // Replace with your actual base URL

        public FaqApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<ApiResponse<FaqCategoryDto>> AddFaqCategoryAsync(FaqCategoryDto faqCategory)
        {
            var json = JsonConvert.SerializeObject(faqCategory);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/v1/faqs/category", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<FaqCategoryDto>(responseContent);
                return new ApiResponse<FaqCategoryDto>
                {
                    Data = result,
                    StatusCode = (int)response.StatusCode
                };
            }
            else
            {
                var errorResult = JsonConvert.DeserializeObject<ContentResult>(responseContent);
                return new ApiResponse<FaqCategoryDto>
                {
                    Error = errorResult,
                    StatusCode = (int)response.StatusCode
                };
            }
        }
    }

    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public ContentResult Error { get; set; }
        public int StatusCode { get; set; }
    }
}