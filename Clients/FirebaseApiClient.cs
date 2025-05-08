using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class FirebaseApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public FirebaseApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<ContentResult>> CreatePushNotificationCategoryAsync(PushNotificationCategoryDto category)
    {
        var json = JsonConvert.SerializeObject(category);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/v1/PushNotificationCategory", content);
        return await ProcessResponseAsync<ContentResult>(response);
    }

    public async Task<ApiResponse<ContentResult>> EditPushNotificationCategoryAsync(PushNotificationCategoryDto category)
    {
        var json = JsonConvert.SerializeObject(category);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync("/api/v1/PushNotificationCategory", content);
        return await ProcessResponseAsync<ContentResult>(response);
    }

    private async Task<ApiResponse<T>> ProcessResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var result = new ApiResponse<T>
        {
            StatusCode = (int)response.StatusCode,
            IsSuccessStatusCode = response.IsSuccessStatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            result.Data = JsonConvert.DeserializeObject<T>(content);
        }
        else
        {
            result.ErrorContent = content;
        }

        return result;
    }
}

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public bool IsSuccessStatusCode { get; set; }
    public T Data { get; set; }
    public string ErrorContent { get; set; }
}