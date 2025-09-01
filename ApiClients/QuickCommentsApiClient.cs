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
        private readonly string _baseUrl;

        public QuickCommentsApiClient(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        public async Task<ApiResponse<QuickCommentResponse>> PostQuickCommentsAsync(QuickCommentRequest request)
        {
            var url = $"{_baseUrl}/api/v1/QuickComments";
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return new ApiResponse<QuickCommentResponse>
            {
                StatusCode = (int)response.StatusCode,
                Content = JsonConvert.DeserializeObject<QuickCommentResponse>(responseContent)
            };
        }
    }

    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public T Content { get; set; }
    }
}