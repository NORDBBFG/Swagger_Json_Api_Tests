using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class UserBadgesApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public UserBadgesApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<List<BadgeDto>>> GetUserBadgesAsync()
    {
        var response = await _httpClient.GetAsync("/api/v1/user/badges");
        return await ApiResponse<List<BadgeDto>>.CreateAsync(response);
    }

    public async Task<ApiResponse<List<BadgeDto>>> AddBadgesToUserAsync(AddBadgesToUserViewModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/v1/user/badges", model);
        return await ApiResponse<List<BadgeDto>>.CreateAsync(response);
    }
}

public class ApiResponse<T>
{
    public T Data { get; set; }
    public bool IsSuccessStatusCode { get; set; }
    public int StatusCode { get; set; }
    public string ReasonPhrase { get; set; }
    public ContentResult ErrorContent { get; set; }

    public static async Task<ApiResponse<T>> CreateAsync(HttpResponseMessage response)
    {
        var apiResponse = new ApiResponse<T>
        {
            IsSuccessStatusCode = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            ReasonPhrase = response.ReasonPhrase
        };

        if (response.IsSuccessStatusCode)
        {
            apiResponse.Data = await response.Content.ReadFromJsonAsync<T>();
        }
        else
        {
            apiResponse.ErrorContent = await response.Content.ReadFromJsonAsync<ContentResult>();
        }

        return apiResponse;
    }
}