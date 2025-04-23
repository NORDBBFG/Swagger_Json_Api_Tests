using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YourNamespace.Models;

namespace YourNamespace.ApiClient
{
    using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using YourNamespace.Models;

namespace YourNamespace.ApiClient
{
    public class FaqApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public FaqApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ApiResponse<List<FaqArticleDto>>> GetFaqArticlesAsync()
        {
            var response = await _httpClient.GetAsync("/api/v1/faqs");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var faqArticles = JsonSerializer.Deserialize<List<FaqArticleDto>>(content, _jsonOptions);
                return new ApiResponse<List<FaqArticleDto>>(response.StatusCode, faqArticles);
            }

            return new ApiResponse<List<FaqArticleDto>>(response.StatusCode, null, content);
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
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public FaqApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
        }

        public async Task<ApiResponse<List<FaqArticleDto>>> GetFaqsByFilterAsync(FaqFilter filter)
        {
            var url = $"{_baseUrl}/api/v1/faqs/filter";
            var content = new StringContent(JsonConvert.SerializeObject(filter), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var faqArticles = JsonConvert.DeserializeObject<List<FaqArticleDto>>(responseContent);
                return new ApiResponse<List<FaqArticleDto>>
                {
                    Data = faqArticles,
                    StatusCode = (int)response.StatusCode
                };
            }
            else
            {
                var errorContent = JsonConvert.DeserializeObject<ContentResult>(responseContent);
                return new ApiResponse<List<FaqArticleDto>>
                {
                    Error = errorContent,
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