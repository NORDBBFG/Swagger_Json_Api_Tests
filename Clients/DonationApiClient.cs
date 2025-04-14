using System;\nusing System.Net.Http;\nusing System.Text;\nusing System.Threading.Tasks;\nusing Newtonsoft.Json;\n\npublic class DonationApiClient\n{\n    private readonly HttpClient _httpClient;\n    private readonly string _baseUrl;\n    private bool _simulateError;\n\n    public DonationApiClient(string baseUrl)\n    {\n        _httpClient = new HttpClient();\n        _baseUrl = baseUrl;\n    }\n\n    public void SimulateError(bool simulate)\n    {\n        _simulateError = simulate;\n    }\n\n    public async Task<ApiResponse<ContentResult>> ConfirmDonationAsync(DonationDto donation)\n    {\n        if (_simulateError)\n        {\n            return new ApiResponse<ContentResult>\n            {\n                StatusCode = System.Net.HttpStatusCode.InternalServerError,\n                Content = new ContentResult\n                {\n                    Content = \"Simulated server error\",\n                    ContentType = \"application/json\",\n                    StatusCode = 500\n                }\n            };\n        }\n\n        var json = JsonConvert.SerializeObject(donation);\n        var content = new StringContent(json, Encoding.UTF8, \"application/json\");\n\n        var response = await _httpClient.PutAsync($\"{_baseUrl}/api/v1/donation/confirm\", content);\n\n        var responseContent = await response.Content.ReadAsStringAsync();\n        var contentResult = JsonConvert.DeserializeObject<ContentResult>(responseContent);\n\n        return new ApiResponse<ContentResult>\n        {\n            StatusCode = response.StatusCode,\n            Content = contentResult\n        };\n    }\n}\n