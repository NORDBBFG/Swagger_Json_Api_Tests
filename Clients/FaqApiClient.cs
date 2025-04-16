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