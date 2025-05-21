using System;
using System.Collections.Generic;
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

        public async Task<List<QuickCommentDto>> CreateQuickCommentAsync(QuickCommentDto quickComment)
        {
            var json = JsonConvert.SerializeObject(quickComment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/v1/QuickComments", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<QuickCommentDto>>(responseContent);
        }
    }
}
