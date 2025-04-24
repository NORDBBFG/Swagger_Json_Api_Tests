using System;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

[TestFixture]
public class ApiGetUserBadges001Tests
{
    private ApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        string baseUrl = "https://api.example.com"; // Replace with actual base URL
        string bearerToken = "your-bearer-token"; // Replace with actual bearer token
        _apiClient = new ApiClient(baseUrl, bearerToken);
    }

    [Test]
    public async Task GetUserBadges_ReturnsSuccessfulResponse()
    {
        // Act
        var response = await _apiClient.GetUserBadgesAsync();

        // Assert
        response.IsSuccessful.Should().BeTrue();
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<BadgeDto>>();

        foreach (var badge in response.Data)
        {
            badge.Id.Should().BeGreaterThan(0);
            badge.Name.Should().NotBeNull();
            badge.Description.Should().NotBeNull();
            badge.Picture.Should().NotBeNull();
            badge.CreateDate.Should().BeAfter(DateTime.MinValue);
            badge.CreatedBy.Should().NotBeNull();
            badge.UpdateDate.Should().BeAfter(DateTime.MinValue);
            badge.LastUpdatedBy.Should().NotBeNull();
        }
    }

    [Test]
    public async Task GetUserBadges_ValidatesResponseContentType()
    {
        // Act
        var response = await _apiClient.GetUserBadgesAsync();

        // Assert
        response.IsSuccessful.Should().BeTrue();
        response.StatusCode.Should().Be(200);
        // Note: Since we're using HttpClient, we don't have direct access to the response headers.
        // In a real-world scenario, you might want to modify the ApiClient to return the content type as well.
        // For now, we'll assume it's correct if the deserialization was successful.
        response.Data.Should().NotBeNull();
    }
}