using System;
using System.Collections.Generic;
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
    public class ApiEndpointDetails001Tests
    {
        private HttpClient _httpClient;
        private QuickCommentApiClient _apiClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
            _apiClient = new QuickCommentApiClient(_httpClient);
        }

        [Test]
        public async Task GetQuickComments_ValidInput_ReturnsSuccessfulResponse()
        {
            // Arrange
            var category = QuickCommentCategory.ReasonsForCancellation;

            // Act
            var response = await _apiClient.GetQuickCommentsAsync(category);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Data.Should().NotBeNull();
            response.Data.Should().BeAssignableTo<List<QuickCommentDto>>();
            response.Data.Should().NotBeEmpty();

            foreach (var comment in response.Data)
            {
                comment.Id.Should().BeGreaterThan(0);
                comment.Comment.Should().NotBeNullOrEmpty();
            }

            response.ErrorResult.Should().BeNull();
        }

        [Test]
        public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
        {
            // Arrange
            var invalidCategory = (QuickCommentCategory)999; // Invalid category

            // Act
            var response = await _apiClient.GetQuickCommentsAsync(invalidCategory);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Data.Should().BeNull();
            response.ErrorResult.Should().NotBeNull();
            response.ErrorResult.StatusCode.Should().Be(400);
            response.ErrorResult.Content.Should().NotBeNullOrEmpty();
        }

        [TearDown]
        public void Teardown()
        {
            _httpClient.Dispose();
        }
    }
}