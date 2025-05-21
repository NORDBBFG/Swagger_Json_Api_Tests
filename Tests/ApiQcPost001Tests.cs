using System;
using System.Collections.Generic;
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
        public async Task CreateQuickComment_ReturnsCollectionOfQuickComments()
        {
            // Arrange
            var newQuickComment = new QuickCommentDto
            {
                Comment = "Test comment",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = await _apiClient.CreateQuickCommentAsync(newQuickComment);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<QuickCommentDto>>();
            result.Should().NotBeEmpty();

            foreach (var quickComment in result)
            {
                quickComment.Id.Should().BeGreaterThan(0);
                quickComment.Comment.Should().NotBeNullOrEmpty();
                quickComment.CreatedAt.Should().NotBe(default(DateTime));
            }
        }
    }
}
