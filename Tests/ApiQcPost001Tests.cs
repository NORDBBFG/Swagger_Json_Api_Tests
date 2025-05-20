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
        private QuickCommentApiClient _apiClient;

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            _apiClient = new QuickCommentApiClient(httpClient);
        }

        [Test]
        public async Task CreateQuickComment_ReturnsCollectionOfQuickComments()
        {
            // Arrange
            var newQuickComment = new QuickCommentDto
            {
                Comment = "This is a test comment",
                CreatedBy = "TestUser",
                CreatedAt = DateTime.UtcNow
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
                Assert.That(quickComment.CreatedBy, Is.Not.Null.And.Not.Empty);
                Assert.That(quickComment.CreatedAt, Is.Not.EqualTo(default(DateTime)));
            }
        }
    }
}