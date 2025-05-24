using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiDnGet132Tests
{
    private BadgeApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new BadgeApiClient(httpClient);
    }

    [Test]
    public async Task GetBadges_ReturnsSuccessfulResponse()
    {
        // Arrange
        // No additional arrangement needed for this test

        // Act
        var response = await _client.GetBadgesAsync();

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<BadgeDto>>();
        response.Data.Should().NotBeEmpty();

        foreach (var badge in response.Data)
        {
            badge.Id.Should().BeGreaterThan(0);
            badge.Name.Should().NotBeNullOrEmpty();
            badge.Description.Should().NotBeNull();
            badge.Picture.Should().NotBeNull();
            badge.CreateDate.Should().NotBe(default(DateTime));
            badge.CreatedBy.Should().NotBeNullOrEmpty();
            badge.UpdateDate.Should().NotBe(default(DateTime));
            badge.LastUpdatedBy.Should().NotBeNullOrEmpty();
        }
    }

    [Test]
    public async Task GetBadges_ReturnsEmptyList_WhenNoBadgesExist()
    {
        // Arrange
        // This test assumes that the API might return an empty list if no badges exist
        // You may need to set up test data or mock the API response for this scenario

        // Act
        var response = await _client.GetBadgesAsync();

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEmpty();
    }

    [Test]
    public async Task GetBadges_ReturnsBadRequest_WhenApiReturns400()
    {
        // Arrange
        // You may need to set up a mock HTTP client or use a tool like WireMock.NET to simulate a 400 response

        // Act
        var response = await _client.GetBadgesAsync();

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().BeNull();
        response.ErrorContent.Should().NotBeNull();
        response.ErrorContent.StatusCode.Should().Be(400);
        response.ErrorContent.Content.Should().NotBeNullOrEmpty();
        response.ErrorContent.ContentType.Should().Be("application/json");
    }

    [Test]
    public async Task GetBadges_ReturnsInternalServerError_WhenApiReturns500()
    {
        // Arrange
        // You may need to set up a mock HTTP client or use a tool like WireMock.NET to simulate a 500 response

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
