using System;\nusing System.Net.Http;\nusing System.Net.Http.Json;\nusing System.Threading.Tasks;\n\npublic class DonationApiClient\n{\n    private readonly HttpClient _httpClient;\n    private bool _simulateError;\n\n    public DonationApiClient(HttpClient httpClient)\n    {\n        _httpClient = httpClient;\n    }\n\n    public void SimulateError(bool simulate)\n    {\n        _simulateError = simulate;\n    }\n\n    public async Task<ApiResponse<ContentResult>> ConfirmDonationAsync(DonationDto donation)\n    {\n        if (_simulateError)\n        {\n            return new ApiResponse<ContentResult>\n            {\n                StatusCode = System.Net.HttpStatusCode.InternalServerError,\n                Content = new ContentResult { Content = \"Simulated error\", StatusCode = 500 }\n            };\n        }\n\n        var response = await _httpClient.PutAsJsonAsync(\"/api/v1/donation/confirm\", donation);\n        var content = await response.Content.ReadFromJsonAsync<ContentResult>();\n\n        return new ApiResponse<ContentResult>\n        {\n            StatusCode = response.StatusCode,\n            Content = content\n        };\n    }\n}\n\npublic class ApiResponse<T>\n{\n    public System.Net.HttpStatusCode StatusCode { get; set; }\n    public T Content { get; set; }\n}