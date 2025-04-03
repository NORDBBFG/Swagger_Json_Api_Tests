using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RestApiNUnitTests.ApiClients
{
    public class DonationTypesApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public DonationTypesApiClient(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        public async Task<HttpResponseMessage> DeleteDonationType(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/v1/donation/types/{id}");
            return response;
        }
    }
}