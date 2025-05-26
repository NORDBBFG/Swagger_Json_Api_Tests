using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using YourNamespace.ApiClients;
using YourNamespace.Models;

namespace YourNamespace.Tests
{
    [TestFixture]
    public class ApiQcPost001Tests
    {
        private QuickCommentsApiClient _apiClient;

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            var baseUrl = "https://your-api-base-url.com"; // Replace with your actual base URL
            _apiClient = new QuickCommentsApiClient(httpClient, baseUrl);
        }

        [Test]
        public async Task PostQuickComments_ReturnsCollectionOfQuickComments()
        {
            // Arrange
            var request = new QuickCommentRequestDto
            {
                Comment = "Test comment",
                UserId = 1
            };

            // Act
            var response = await _apiClient.PostQuickCommentsAsync(request);

            // Assert
            response.StatusCode.Should().Be(200);
            response.Data.Should().NotBeNull();
            response.Data.Should().BeAssignableTo<List<QuickCommentResponseDto>>();

            foreach (var quickComment in response.Data)
            {
                quickComment.Id.Should().BeGreaterThan(0);
                quickComment.Comment.Should().NotBeNullOrEmpty();
                quickComment.UserId.Should().BeGreaterThan(0);
                quickComment.Timestamp.Should().NotBe(default(DateTime));
            }
        }
    }
}
