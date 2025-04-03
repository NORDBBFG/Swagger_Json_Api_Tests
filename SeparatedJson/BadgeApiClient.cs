using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RestApiNUnitTests
{
    public class BadgeApiClient
    {
        private readonly HttpClient _httpClient;
        private bool _simulateError = false;
        private bool _invalidParameter = false;

        public BadgeApiClient(string baseUrl)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public async Task<ApiResponse<List<BadgeDto>>> GetBadgesAsync()
        {
            if (_simulateError)
            {
                return new ApiResponse<List<BadgeDto>> { StatusCode = System.Net.HttpStatusCode.InternalServerError, ErrorMessage = "Simulated server error" };
            }

            if (_invalidParameter)
            {
                return new ApiResponse<List<BadgeDto>> { StatusCode = System.Net.HttpStatusCode.BadRequest, ErrorMessage = "Invalid parameter" };
            }

            var response = await _httpClient.GetAsync("/api/v1/badges");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var badges = JsonConvert.DeserializeObject<List<BadgeDto>>(content);
                return new ApiResponse<List<BadgeDto>> { StatusCode = response.StatusCode, Data = badges };
            }

            return new ApiResponse<List<BadgeDto>> { StatusCode = response.StatusCode, ErrorMessage = content };
        }

        public void SimulateServerError()
        {
            _simulateError = true;
        }

        public void SetInvalidParameter()
        {
            _invalidParameter = true;
        }
    }

    public class ApiResponse<T>
    {
        public System.Net.HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
    }
}
