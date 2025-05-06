using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DonorApp.ApiClient;
using DonorApp.Services.DTO;
using NUnit.Framework;
using FluentAssertions;

namespace DonorApp.Tests
{
    [TestFixture]
    public class ApiTc001Tests
    {
        private DonationTypesApiClient _client;

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            // Add any necessary headers, like authentication token
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer <valid_token>");
            _client = new DonationTypesApiClient(httpClient);
        }

        [Test]
        public async Task GetDonationTypes_ReturnsSuccessfulResponse()
        {
            // Arrange
            // No additional arrangement needed as the client is set up in the Setup method

            // Act
            var result = await _client.GetDonationTypesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<DonationTypeDto>>();
            result.Should().NotBeEmpty();

            foreach (var donationType in result)
            {
                donationType.Id.Should().BeGreaterThan(0);
                donationType.Name.Should().NotBeNullOrEmpty();
                donationType.Description.Should().NotBeNullOrEmpty();
            }

            // Example assertion for a specific donation type (adjust as needed)
            result.Should().Contain(dt => 
                dt.Id == 1 && 
                dt.Name == "Blood Donation" && 
                dt.Description == "Donation of blood or blood components");
        }
    }
}
