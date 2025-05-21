using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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

        public async Task<List<QuickCommentDto>> CreateQuickCommentAsync(QuickCommentDto quickComment)
        {
            var json = JsonSerializer.Serialize(quickComment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/QuickComments", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<QuickCommentDto>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
