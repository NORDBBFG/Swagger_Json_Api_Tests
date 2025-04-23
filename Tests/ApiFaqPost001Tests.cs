using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using YourNamespace.ApiClients;
using YourNamespace.Models;
using FluentAssertions;

namespace YourNamespace.Tests
{
    [TestFixture]
    public class ApiFaqPost001Tests
    {
        private FaqApiClient _faqApiClient;

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            _faqApiClient = new FaqApiClient(httpClient);
        }

        [Test]
        public async Task AddFaqCategory_ValidData_ReturnsSuccessfulResponse()
        {
            // Arrange
            var newCategory = new FaqCategoryDto
            {
                Name = "Test Category",
                Picture = "https://example.com/test-picture.jpg",
                IsPermanent = false
            };

            // Act
            var response = await _faqApiClient.AddFaqCategoryAsync(newCategory);

            // Assert
            response.StatusCode.Should().Be(200);
            response.Data.Should().NotBeNull();
            response.Data.Id.Should().BeGreaterThan(0);
            response.Data.Name.Should().Be(newCategory.Name);
            response.Data.Picture.Should().Be(newCategory.Picture);
            response.Data.IsPermanent.Should().Be(newCategory.IsPermanent);
            response.Data.Created.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            response.Data.Updated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            response.Data.CreatedBy.Should().NotBeNullOrEmpty();
            response.Data.LastUpdatedBy.Should().NotBeNullOrEmpty();
            response.Data.CreatedByName.Should().NotBeNullOrEmpty();
            response.Data.LastUpdatedByName.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task AddFaqCategory_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var invalidCategory = new FaqCategoryDto
            {
                Name = "", // Invalid: empty name
                Picture = "not-a-valid-url",
                IsPermanent = true
            };

            // Act
            var response = await _faqApiClient.AddFaqCategoryAsync(invalidCategory);

            // Assert
            response.StatusCode.Should().Be(400);
            response.Error.Should().NotBeNull();
            response.Error.Content.Should().Contain("Bad request");
        }

        [Test]
        public async Task AddFaqCategory_ServerError_ReturnsInternalServerError()
        {
            // Arrange
            var category = new FaqCategoryDto
            {
                Name = "Server Error Test",
                Picture = "https://example.com/server-error-test.jpg",
                IsPermanent = false
            };

            // Simulate a server error by modifying the base URL to an invalid one
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://invalid-url-to-force-server-error.com");
            var faqApiClientWithInvalidUrl = new FaqApiClient(httpClient);

            // Act
            var response = await faqApiClientWithInvalidUrl.AddFaqCategoryAsync(category);

            // Assert
            response.StatusCode.Should().Be(500);
            response.Error.Should().NotBeNull();
            response.Error.Content.Should().Contain("Internal server error");
        }
    }
}