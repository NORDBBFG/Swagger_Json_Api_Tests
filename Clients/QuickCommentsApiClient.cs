using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YourNamespace.Models;

namespace YourNamespace.ApiClients
{
    public class QuickCommentsApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://your-api-base-url.com"; // Replace with actual base URL

        public QuickCommentsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<ApiResponse<QuickCommentResponseDto>> CreateQuickCommentAsync(QuickCommentRequestDto request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/v1/QuickComments", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return new ApiResponse<QuickCommentResponseDto>
            {
                StatusCode = (int)response.StatusCode,
                Data = JsonConvert.DeserializeObject<QuickCommentResponseDto>(responseContent)
            };
        }
    }

    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
    }
}
