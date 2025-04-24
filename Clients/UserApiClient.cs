using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class UserApiClient
{
    private readonly HttpClient _httpClient;

    public UserApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<ApiResponse<List<Holiday>>> GetHolidaysAsync()
    {
        var response = await _httpClient.GetAsync("/api/v1/user/Holidays");
        
        if (response.IsSuccessStatusCode)
        {
            var holidays = await response.Content.ReadFromJsonAsync<List<Holiday>>();
            return new ApiResponse<List<Holiday>>(holidays, response.StatusCode);
        }
        
        var errorContent = await response.Content.ReadFromJsonAsync<ContentResult>();
        return new ApiResponse<List<Holiday>>(null, response.StatusCode, errorContent);
    }
}

public class ApiResponse<T>
{
    public T Data { get; }
    public System.Net.HttpStatusCode StatusCode { get; }
    public ContentResult ErrorContent { get; }

    public ApiResponse(T data, System.Net.HttpStatusCode statusCode, ContentResult errorContent = null)
    {
        Data = data;
        StatusCode = statusCode;
        ErrorContent = errorContent;
    }
}