using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(string baseUrl, string bearerToken)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
    }

    public async Task<ApiResponse<List<BadgeDto>>> GetUserBadgesAsync()
    {
        var response = await _httpClient.GetAsync("/api/v1/user/badges");
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var badges = JsonConvert.DeserializeObject<List<BadgeDto>>(content);
            return new ApiResponse<List<BadgeDto>>
            {
                Data = badges,
                StatusCode = (int)response.StatusCode,
                IsSuccessful = true
            };
        }
        else
        {
            var errorContent = JsonConvert.DeserializeObject<ContentResult>(content);
            return new ApiResponse<List<BadgeDto>>
            {
                ErrorContent = errorContent,
                StatusCode = (int)response.StatusCode,
                IsSuccessful = false
            };
        }
    }
}

public class ApiResponse<T>
{
    public T Data { get; set; }
    public ContentResult ErrorContent { get; set; }
    public int StatusCode { get; set; }
    public bool IsSuccessful { get; set; }
}