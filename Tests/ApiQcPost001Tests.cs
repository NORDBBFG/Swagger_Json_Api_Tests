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
    public class ApiQcPost001Tests
    {
        private QuickCommentsApiClient _apiClient;

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            _apiClient = new QuickCommentsApiClient(httpClient);
        }

        [Test]
        public async Task CreateQuickComment_ReturnsCorrectResponse()
        {
            // Arrange
            var request = new QuickCommentRequestDto
            {
                Comment = "This is a test comment",
                UserId = 1
            };

            // Act
            var response = await _apiClient.CreateQuickCommentAsync(request);

            // Assert
            response.StatusCode.Should().Be(200);
            response.Data.Should().NotBeNull();
            response.Data.Should().BeOfType<QuickCommentResponseDto>();
            response.Data.Comment.Should().Be(request.Comment);
            response.Data.UserId.Should().Be(request.UserId);
            response.Data.Id.Should().BeGreaterThan(0);
            response.Data.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}