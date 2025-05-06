using System;
using System.Net.Http;
using System.Threading.Tasks;
using DonorApp.ApiClient;
using NUnit.Framework;
using FluentAssertions;

namespace DonorApp.Tests
{
    [TestFixture]
    public class ApiTc001Tests
    {
        private HttpClient _httpClient;
        private DonationTypesApiClient _apiClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient = new DonationTypesApiClient(_httpClient);
        }

        [Test]
        public async Task CreateUser_ReturnsCreatedStatus()
        {
            // Arrange
            var user = new
            {
                name = "testuser",
                age = 25
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("https://example.com/api/users", user);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<UserResponse>(content);

            result.Should().NotBeNull();
            result.User.Should().NotBeNull();
            result.User.Id.Should().BeGreaterThan(0);
            result.User.Username.Should().Be("testuser");
            result.User.Email.Should().Be("testuser@example.com");
        }

        private class UserResponse
        {
            public User User { get; set; }
        }

        private class User
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}