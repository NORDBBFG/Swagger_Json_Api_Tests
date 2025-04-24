using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using YourNamespace.ApiClients;
using YourNamespace.Models;

namespace YourNamespace.Tests
{
    [TestFixture]
    public class ApiGetRegions001Tests
    {
        private HttpClient _httpClient;
        private LocationApiClient _locationApiClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://your-api-base-url.com")
            };
            _locationApiClient = new LocationApiClient(_httpClient);
        }

        [Test]
        public async Task GetRegions_ReturnsSuccessfulResponse()
        {
            // Arrange
            // No additional arrangement needed as we're testing a GET request without parameters

            // Act
            var response = await _locationApiClient.GetRegionsAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Data.Should().NotBeNull();
            response.Data.Should().BeAssignableTo<List<RegionDto>>();

            foreach (var region in response.Data)
            {
                region.Id.Should().BeGreaterThan(0);
                region.Name.Should().BeOfType<string>().Or.BeNull();
            }
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}
