using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class FirebaseApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public FirebaseApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
    }

    public async Task<ApiResponse<ContentResult>> CreatePushNotificationAsync(PushNotificationDto notification)
    {
        var json = JsonConvert.SerializeObject(notification);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/PushNotification", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        return new ApiResponse<ContentResult>
        {
            StatusCode = (int)response.StatusCode,
            Data = JsonConvert.DeserializeObject<ContentResult>(responseContent)
        };
    }

    public async Task<ApiResponse<ContentResult>> EditPushNotificationAsync(PushNotificationDto notification)
    {
        var json = JsonConvert.SerializeObject(notification);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{_baseUrl}/api/v1/PushNotification", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        return new ApiResponse<ContentResult>
        {
            StatusCode = (int)response.StatusCode,
            Data = JsonConvert.DeserializeObject<ContentResult>(responseContent)
        };
    }

    public async Task<ApiResponse<ContentResult>> DeletePushNotificationAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/v1/PushNotification?id={id}");
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