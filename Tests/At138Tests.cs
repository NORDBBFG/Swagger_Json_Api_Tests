using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class At138Tests
{
    private BadgeApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new BadgeApiClient(httpClient);
    }

    [Test]
    public async Task GetBadge_ReturnsSuccessfulResponse()
    {
        // Arrange
        // No arrangement needed for this test

        // Act
        var response = await _client.GetBadgeAsync();

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().NotBeNullOrEmpty();
        response.Data.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task GetBadge_ReturnsBadRequest()
    {
        // Arrange
        // You might need to set up a condition that would trigger a bad request
        // This could involve modifying the client or using a mock server

        // Act
        var response = await _client.GetBadgeAsync();

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().NotBeNullOrEmpty();
        response.Data.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task GetBadge_ReturnsInternalServerError()
    {
        // Arrange
        // You might need to set up a condition that would trigger an internal server error
        // This could involve modifying the client or using a mock server

        // Act
        var response = await _client.GetBadgeAsync();

        // Assert
        response.StatusCode.Should().Be(500);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().NotBeNullOrEmpty();
        response.Data.StatusCode.Should().Be(500);
    }
}