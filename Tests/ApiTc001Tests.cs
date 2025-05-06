using System.Collections.Generic;
using System.Net;
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
            _client = new DonationTypesApiClient(httpClient);
        }

        [Test]
        public async Task GetDonationTypes_ReturnsSuccessfulResponse()
        {
            // Arrange
            // No additional arrangement needed as the client is set up in the Setup method

            // Act
            var response = await _client.GetDonationTypesAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Data.Should().NotBeNull();
            response.Data.Should().BeAssignableTo<List<DonationTypeDto>>();
            
            foreach (var donationType in response.Data)
            {
                donationType.Id.Should().BeGreaterThan(0);
                donationType.Name.Should().NotBeNullOrEmpty();
                donationType.Description.Should().NotBeNull();
            }

            // Example assertion for a specific donation type (adjust as needed)
            response.Data.Should().Contain(dt => 
                dt.Id == 1 && 
                dt.Name == "Blood Donation" && 
                dt.Description == "Donation of blood for medical use");
        }
    }
}
