using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using DonorApp.ApiClient;
using DonorApp.Services.DTO;

namespace DonorApp.Tests
{
    [TestFixture]
    public class ApiPostDonationTypes001Tests
    {
        private DonationTypesApiClient _client;
        private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            _client = new DonationTypesApiClient(httpClient, BaseUrl);
        }

        [Test]
        public async Task AddDonationType_ValidData_ReturnsSuccessfulResponse()
        {
            // Arrange
            var newDonationType = new DonationTypeDto
            {
                Name = "Test Donation Type",
                ShortDescription = "Short description",
                LongDescription = "Long description",
                IconData = "base64encodedicon",
                CreateDate = DateTime.UtcNow,
                CreatedBy = "TestUser",
                UpdateDate = DateTime.UtcNow,
                LastUpdatedBy = "TestUser",
                HeaderColor = "#FF0000"
            };

            // Act
            var response = await _client.AddDonationTypeAsync(newDonationType);

            // Assert
            response.StatusCode.Should().Be(200);
            response.Data.Should().NotBeNull();
            response.Data.Content.Should().NotBeNullOrEmpty();
            response.Data.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task AddDonationType_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var invalidDonationType = new DonationTypeDto
            {
                // Missing required fields
            };

            // Act
            var response = await _client.AddDonationTypeAsync(invalidDonationType);

            // Assert
            response.StatusCode.Should().Be(400);
            response.Data.Should().NotBeNull();
            response.Data.Content.Should().NotBeNullOrEmpty();
            response.Data.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task AddDonationType_ServerError_ReturnsInternalServerError()
        {
            // Arrange
            var donationType = new DonationTypeDto
            {
                Name = "Server Error Test",
                ShortDescription = "This should trigger a server error",
                // Add other required fields
            };

            // Act
            var response = await _client.AddDonationTypeAsync(donationType);

            // Assert
            response.StatusCode.Should().Be(500);
            response.Data.Should().NotBeNull();
            response.Data.Content.Should().NotBeNullOrEmpty();
            response.Data.StatusCode.Should().Be(500);
        }
    }
}