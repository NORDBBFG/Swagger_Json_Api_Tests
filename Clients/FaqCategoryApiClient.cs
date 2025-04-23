using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class FaqCategoryApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public FaqCategoryApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
    }

    public async Task<ApiResponse<FaqCategoryDto>> AddFaqCategoryAsync(FaqCategoryDto faqCategory)
    {
        var url = $"{_baseUrl}/api/v1/faqs/category";
        var json = JsonConvert.SerializeObject(faqCategory);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var result = JsonConvert.DeserializeObject<FaqCategoryDto>(responseContent);
            return new ApiResponse<FaqCategoryDto>
            {
                StatusCode = (int)response.StatusCode,
                Data = result
            };
        }
        else
        {
            var errorResult = JsonConvert.DeserializeObject<ContentResult>(responseContent);
            return new ApiResponse<FaqCategoryDto>
            {
                StatusCode = (int)response.StatusCode,
                ErrorContent = errorResult
            };
        }
    }
}

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public T Data { get; set; }
    public ContentResult ErrorContent { get; set; }
}