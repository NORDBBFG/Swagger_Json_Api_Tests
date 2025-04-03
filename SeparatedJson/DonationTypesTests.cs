using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using RestApiNUnitTests.ApiClients;
using RestApiNUnitTests.Models;

namespace RestApiNUnitTests
{
    [TestFixture]
    public class DonationTypesTests
    {
        private DonationTypesApiClient _client;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
            _client = new DonationTypesApiClient(_httpClient, "http://api-base-url");
        }

        [Test]
        public async Task DeleteDonationType_ValidId_ReturnsOk()
        {
            // Arrange
            int validId = 1;

            // Act
            var response = await _client.DeleteDonationType(validId);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task DeleteDonationType_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;

            // Act
            var response = await _client.DeleteDonationType(invalidId);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task DeleteDonationType_NonExistentId_ReturnsNotFound()
        {
            // Arrange
            int nonExistentId = 99999;

            // Act
            var response = await _client.DeleteDonationType(nonExistentId);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}