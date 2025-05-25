using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;

[TestFixture]
public class ApiBadgesGetTests
{
    private BadgeApiClient _client;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new BadgeApiClient(httpClient, BaseUrl);
    }

    [Test]
    public async Task AT148_GetBadges_ReturnsSuccessfulResponse()
    {
        // Arrange

        // Act
        var response = await _client.GetBadgesAsync();

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<List<BadgeDto>>();
        response.Data.Should().NotBeEmpty();

        // Validate structure and key fields of the first badge
        var firstBadge = response.Data.First();
        firstBadge.Id.Should().BeGreaterThan(0);
        firstBadge.Name.Should().NotBeNullOrEmpty();
        firstBadge.CreateDate.Should().BeBefore(DateTime.UtcNow);
        firstBadge.UpdateDate.Should().BeBefore(DateTime.UtcNow);
    }

    [Test]
    public async Task AT149_GetBadges_ReturnsInternalServerError()
    {
        // Arrange
        // Note: This test assumes that we have a way to simulate a 500 error.
        // In a real scenario, you might need to mock the HttpClient or use a test server.

        // Act
        var response = await _client.GetBadgesAsync();

        // Assert
        response.StatusCode.Should().Be(500);
        response.Data.Should().BeNull();
        response.ErrorContent.Should().NotBeNull();
        response.ErrorContent.StatusCode.Should().Be(500);
        response.ErrorContent.Content.Should().NotBeNullOrEmpty();
        response.ErrorContent.ContentType.Should().Be("application/json");
    }
}