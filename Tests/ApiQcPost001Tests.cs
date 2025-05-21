using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
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
        public async Task CreateQuickComment_ReturnsCollectionOfQuickComments()
        {
            // Arrange
            var newQuickComment = new QuickCommentDto
            {
                Comment = "Test comment",
                Author = "Test Author",
                Timestamp = DateTime.UtcNow
            };

            // Act
            var result = await _apiClient.CreateQuickCommentAsync(newQuickComment);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<List<QuickCommentDto>>());
            Assert.That(result.Count, Is.GreaterThan(0));

            foreach (var quickComment in result)
            {
                Assert.That(quickComment.Id, Is.GreaterThan(0));
                Assert.That(quickComment.Comment, Is.Not.Null.And.Not.Empty);
                Assert.That(quickComment.Author, Is.Not.Null.And.Not.Empty);
                Assert.That(quickComment.Timestamp, Is.Not.EqualTo(default(DateTime)));
            }
        }
    }
}
