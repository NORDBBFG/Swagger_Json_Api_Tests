using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using YourNamespace.Models;

namespace YourNamespace.ApiClients
{
    public class LocationApiClient
    {
        private readonly HttpClient _httpClient;

        public LocationApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<ApiResponse<List<RegionDto>>> GetRegionsAsync()
        {
            var response = await _httpClient.GetAsync("/api/v1/regions");
            
            if (response.IsSuccessStatusCode)
            {
                var regions = await response.Content.ReadFromJsonAsync<List<RegionDto>>();
                return new ApiResponse<List<RegionDto>>(response.StatusCode, regions);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                var errorContent = await response.Content.ReadFromJsonAsync<ContentResult>();
                return new ApiResponse<List<RegionDto>>(response.StatusCode, default, errorContent);
            }
            else
            {
                return new ApiResponse<List<RegionDto>>(response.StatusCode);
            }
        }
    }

    public class ApiResponse<T>
    {
        public System.Net.HttpStatusCode StatusCode { get; }
        public T Data { get; }
        public ContentResult ErrorContent { get; }

        public ApiResponse(System.Net.HttpStatusCode statusCode, T data = default, ContentResult errorContent = null)
        {
            StatusCode = statusCode;
            Data = data;
            ErrorContent = errorContent;
        }
    }
}
