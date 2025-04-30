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
        var responseContent = await response.Content.ReadAsStringAsync();

        return new ApiResponse<ContentResult>
        {
            StatusCode = (int)response.StatusCode,
            Data = JsonConvert.DeserializeObject<ContentResult>(responseContent)
        };
    }

    public async Task<ApiResponse<ContentResult>> CreatePasswordAsync(CreatePasswordViewModel model)
    {
        var json = JsonConvert.SerializeObject(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/v1/user/password", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        return new ApiResponse<ContentResult>
        {
            StatusCode = (int)response.StatusCode,
            Data = JsonConvert.DeserializeObject<ContentResult>(responseContent)
        };
    }
}

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public T Data { get; set; }
}