using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class FaqCategoryApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public FaqCategoryApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<FaqCategoryDto>> AddFaqCategoryAsync(FaqCategoryDto faqCategory)
    {
        var json = JsonConvert.SerializeObject(faqCategory);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/v1/faqs/category", content);
        return await CreateApiResponseAsync<FaqCategoryDto>(response);
    }

    public async Task<ApiResponse<FaqCategoryDto>> EditFaqCategoryAsync(FaqCategoryDto faqCategory)
    {
        var json = JsonConvert.SerializeObject(faqCategory);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync("/api/v1/faqs/category", content);
        return await CreateApiResponseAsync<FaqCategoryDto>(response);
    }

    private async Task<ApiResponse<T>> CreateApiResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var apiResponse = new ApiResponse<T>
        {
            StatusCode = (int)response.StatusCode,
            IsSuccessStatusCode = response.IsSuccessStatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            apiResponse.Data = JsonConvert.DeserializeObject<T>(content);
        }
        else
        {
            apiResponse.ErrorContent = JsonConvert.DeserializeObject<ContentResult>(content);
        }

        return apiResponse;
    }
}

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public bool IsSuccessStatusCode { get; set; }
    public T Data { get; set; }
    public ContentResult ErrorContent { get; set; }
}