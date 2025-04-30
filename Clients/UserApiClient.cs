using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class UserApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public UserApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<ContentResult>> ChangePasswordAsync(UserChangePasswordModel model)
    {
        var json = JsonConvert.SerializeObject(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync("/api/v1/user/password", content);
        return await CreateApiResponseAsync<ContentResult>(response);
    }

    public async Task<ApiResponse<ContentResult>> CreatePasswordAsync(CreatePasswordViewModel model)
    {
        var json = JsonConvert.SerializeObject(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/v1/user/password", content);
        return await CreateApiResponseAsync<ContentResult>(response);
    }

    private async Task<ApiResponse<T>> CreateApiResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<T>(content);

        return new ApiResponse<T>
        {
            StatusCode = (int)response.StatusCode,
            IsSuccessStatusCode = response.IsSuccessStatusCode,
            Content = result
        };
    }
}

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public bool IsSuccessStatusCode { get; set; }
    public T Content { get; set; }
}