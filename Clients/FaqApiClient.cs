using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YourNamespace.Models;

namespace YourNamespace.ApiClient
{
    public class FaqApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public FaqApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
        }

        public async Task<ApiResponse<FaqCategoryDto>> EditFaqCategoryAsync(FaqCategoryDto faqCategory)
        {
            var url = $"{_baseUrl}/api/v1/faqs/category";
            var content = new StringContent(JsonConvert.SerializeObject(faqCategory), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var updatedFaqCategory = JsonConvert.DeserializeObject<FaqCategoryDto>(responseContent);
                return new ApiResponse<FaqCategoryDto>(response.StatusCode, updatedFaqCategory);
            }
            else
            {
                var errorContent = JsonConvert.DeserializeObject<ContentResult>(responseContent);
                return new ApiResponse<FaqCategoryDto>(response.StatusCode, null, errorContent);
            }
        }
    }

    public class ApiResponse<T>
    {
        public System.Net.HttpStatusCode StatusCode { get; }
        public T Data { get; }
        public ContentResult ErrorContent { get; }

        public ApiResponse(System.Net.HttpStatusCode statusCode, T data, ContentResult errorContent = null)
        {
            StatusCode = statusCode;
            Data = data;
            ErrorContent = errorContent;
        }
    }
}