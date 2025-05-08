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

    public async Task<ContentResult> EditPushNotificationCategoryAsync(PushNotificationCategoryDto category)
    {
        var json = JsonConvert.SerializeObject(category);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{_baseUrl}/api/v1/PushNotificationCategory", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var contentResult = JsonConvert.DeserializeObject<ContentResult>(responseContent);

        if (contentResult == null)
        {
            contentResult = new ContentResult
            {
                Content = responseContent,
                StatusCode = (int)response.StatusCode
            };
        }

        return contentResult;
    }
}