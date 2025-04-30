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
        var response = await SendRequestAsync<ContentResult>(HttpMethod.Put, "/api/v1/user/password", model);
        return response;
    }

    public async Task<ApiResponse<ContentResult>> CreatePasswordAsync(CreatePasswordViewModel model)
    {
        var response = await SendRequestAsync<ContentResult>(HttpMethod.Post, "/api/v1/user/password", model);
        return response;
    }

    private async Task<ApiResponse<T>> SendRequestAsync<T>(HttpMethod method, string endpoint, object data = null)
    {
        var request = new HttpRequestMessage(method, `${_baseUrl}${endpoint}`);

        if (data != null)
        {
            var json = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        return new ApiResponse<T>
        {
            StatusCode = (int)response.StatusCode,
            Content = JsonConvert.DeserializeObject<T>(content)
        };
    }
}

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public T Content { get; set; }
}