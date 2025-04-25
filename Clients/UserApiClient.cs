using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class UserApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public UserApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
    }

    public async Task<ApiResponse<ContentResult>> ChangePasswordAsync(UserChangePasswordModel model)
    {
        var json = JsonConvert.SerializeObject(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{_baseUrl}/api/v1/user/password", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var contentResult = JsonConvert.DeserializeObject<ContentResult>(responseContent);

        return new ApiResponse<ContentResult>
        {
            StatusCode = (int)response.StatusCode,
            Data = contentResult
        };
    }
}

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public T Data { get; set; }
}
