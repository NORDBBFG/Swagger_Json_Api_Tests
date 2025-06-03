// ApiDnGet159Tests.cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using DonorApp.ApiClient;
using DonorApp.Services.DTO;
using NUnit.Framework;
using FluentAssertions;

namespace DonorApp.Tests
{
    [TestFixture]
    public class ApiDnGet159Tests
    {
        private HttpClient _httpClient;
        private DonationTypesApiClient _apiClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://your-api-base-url.com/")
            };
            _apiClient = new DonationTypesApiClient(_httpClient);
        }

        [Test]
        public async Task GetDonationTypes_ReturnsSuccessfulResponse()
        {
            // Act
            var response = await _apiClient.GetDonationTypesAsync();

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(200);
            response.Data.Should().NotBeNull();
            response.Data.Should().BeOfType<List<DonationTypeDto>>();
        }

        [Test]
        public async Task GetDonationTypes_ReturnsExpectedData()
        {
            // Act
            var response = await _apiClient.GetDonationTypesAsync();

            // Assert
            response.Data.Should().NotBeEmpty();
            foreach (var donationType in response.Data)
            {
                donationType.Id.Should().NotBeEmpty();
                donationType.Name.Should().NotBeNullOrWhiteSpace();
                // Add more specific assertions based on expected data
            }
        }

        [Test]
        public async Task GetDonationTypes_HandlesServerError()
        {
            // Arrange
            _httpClient.BaseAddress = new Uri("https://invalid-url.com/");

            // Act
            var response = await _apiClient.GetDonationTypesAsync();

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(500);
            response.Data.Should().BeNull();
        }
    }
}
