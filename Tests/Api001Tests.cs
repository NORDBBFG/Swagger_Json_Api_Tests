using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using YourNamespace.ApiClients;
using YourNamespace.Models;

namespace YourNamespace.Tests
{
    [TestFixture]
    public class Api001Tests
    {
        private QuickCommentApiClient _apiClient;
        private const string BaseUrl = "https://your-api-base-url.com";
        private const string AuthToken = "your-auth-token";

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            _apiClient = new QuickCommentApiClient(httpClient, BaseUrl);
        }

        [Test]
        public async Task CreateQuickComment_WithValidData_ReturnsSuccessfulResponse()
        {
            // Arrange
            var parameter1 = "Example Value 1";
            var parameter2 = "Example Value 2";
            var parameter3 = "Example Value 3";

            // Act
            var response = await _apiClient.CreateQuickCommentAsync(parameter1, parameter2, parameter3, AuthToken);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.IsSuccessful, Is.True);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Field1, Is.EqualTo("Expected Value 1"));
            Assert.That(response.Data.Field2, Is.EqualTo("Expected Value 2"));
            Assert.That(response.Data.Field3, Is.EqualTo("Expected Value 3"));
        }
    }
}
