using System;
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
            var request = new QuickCommentRequest
            {
                Comment = "This is a test comment",
                Author = "Test Author"
            };

            // Act
            var response = await _apiClient.PostQuickCommentsAsync(request);

            // Assert
            response.StatusCode.Should().Be(200);
            response.Content.Should().NotBeNull();
            response.Content.Comments.Should().NotBeNull().And.NotBeEmpty();

            foreach (var comment in response.Content.Comments)
            {
                comment.Id.Should().NotBeNullOrEmpty();
                comment.Comment.Should.
                Be(request.Comment);
                comment.Author.Should().Be(request.Author);
                comment.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
            }
        }
    }
}