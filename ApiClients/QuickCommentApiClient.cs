using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YourNamespace.Models;

namespace YourNamespace.ApiClients
{
    public class QuickCommentApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public QuickCommentApiClient(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        public async Task<ApiResponse<QuickCommentDto>> CreateQuickCommentAsync(string parameter1, string parameter2, string parameter3, string authToken)
        {
            var request = new
            {
                Parameter1 = parameter1,
                Parameter2 = parameter2,
                Parameter3 = parameter3
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/QuickComments", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var quickComment = JsonConvert.DeserializeObject<QuickCommentDto>(responseContent);

            return new ApiResponse<QuickCommentDto>
            {
                StatusCode = (int)response.StatusCode,
                Data = quickComment,
                IsSuccessful = response.IsSuccessStatusCode
            };
        }
    }

    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
