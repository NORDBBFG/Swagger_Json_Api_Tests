using System;
using System.Net.Http;
using System.Threading.Tasks;
using DonorApp.ApiClient;
using DonorApp.Services.DTO;
using NUnit.Framework;
using FluentAssertions;

namespace DonorApp.Tests.Api
{
    [TestFixture]
    public class DonationTypesApiTests
    {
        private DonationTypesApiClient _client;

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            _client = new DonationTypesApiClient(httpClient);
        }

        [Test]
        public async Task GetDonationTypes_ShouldReturnAllDonationTypes()
        {
            // Arrange

            // Act
            var response = await _client.GetDonationTypesAsync();

            // Assert
            response.StatusCode.Should().Be(200);
            response.Data.Should().NotBeNull();
            response.Data.Should().BeOfType<List<DonationTypeDto>>();
            response.Data.Should().NotBeEmpty();

            foreach (var donationType in response.Data)
            {
                donationType.Id.Should().NotBeEmpty();
                donationType.Name.Should().NotBeNullOrWhiteSpace();
                donationType.Description.Should().NotBeNull();
            }
        }

        [Test]
        public async Task GetDonationTypes_ShouldHandleServerError()
        {
            // Arrange
            // You might need to mock the HttpClient to simulate a 500 error

            // Act
            var response = await _client.GetDonationTypesAsync();

            // Assert
            response.StatusCode.Should().Be(500);
            response.Data.Should().BeNull();
            response.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        }
    }
}
