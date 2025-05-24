using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

[TestFixture]
public class BadgeApiTests
{
    private BadgeApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new BadgeApiClient(httpClient);
    }

    [Test]
    public async Task AT140_CreateBadge_SuccessfulCreation()
    {
        // Arrange
        var newBadge = new BadgeDto
        {
            Name = "Test Badge",
            Description = "This is a test badge",
            Picture = "https://example.com/test-badge.png",
            CreatedBy = "Test User"
        };

        // Act
        var response = await _client.CreateBadgeAsync(newBadge);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().Contain("Badge successfully added");
        response.Data.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task AT141_CreateBadge_InvalidPayload()
    {
        // Arrange
        var invalidBadge = new BadgeDto
        {
            // Missing required fields
        };

        // Act
        var response = await _client.CreateBadgeAsync(invalidBadge);

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().Contain("Bad request");
        response.Data.StatusCode.Should().Be(400);
    }
}
