using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using Moq;
using Moq.Protected;

[TestFixture]
public class ApiDnGet001Tests
{
    private BadgeApiClient _client;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;

    [SetUp]
    public void Setup()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _client = new BadgeApiClient(httpClient);
    }

    [Test]
    public async Task GetBadges_ReturnsSuccessfulResponse()
    {
        // Arrange
        var expectedBadges = new List<BadgeDto>
        {
            new BadgeDto
            {
                Id = 1,
                Name = "Test Badge",
                Description = "Test Description",
                Picture = "test.jpg",
                CreateDate = DateTime.UtcNow,
                CreatedBy = "TestUser",
                UpdateDate = DateTime.UtcNow,
                LastUpdatedBy = "TestUser"
            }
        };

        var jsonResponse = JsonSerializer.Serialize(expectedBadges);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        // Act
        var response = await _client.GetBadgesAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().HaveCount(1);
        response.Data[0].Should().BeEquivalentTo(expectedBadges[0]);
    }

    [Test]
    public async Task GetBadges_ReturnsEmptyList()
    {
        // Arrange
        var emptyList = new List<BadgeDto>();
        var jsonResponse = JsonSerializer.Serialize(emptyList);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        // Act
        var response = await _client.GetBadgesAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEmpty();
    }

    [Test]
    public async Task GetBadges_ReturnsBadRequest()
    {
        // Arrange
        var errorContent = new ContentResult
        {
            Content = "Bad Request",
            ContentType = "application/json",
            StatusCode = 400
        };

        var jsonResponse = JsonSerializer.Serialize(errorContent);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(jsonResponse)
            });

        // Act
        var response = await _client.GetBadgesAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Data.Should().BeNull();
        response.ErrorResult.Should().NotBeNull();
        response.ErrorResult.Should().BeEquivalentTo(errorContent);
    }

    [Test]
    public async Task GetBadges_ReturnsInternalServerError()
    {
        // Arrange
        var errorContent = new ContentResult
        {
            Content = "Internal Server Error",
            ContentType = "application/json",
            StatusCode = 500
        };

        var jsonResponse = JsonSerializer.Serialize(errorContent);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(jsonResponse)
            });

        // Act
        var response = await _client.GetBadgesAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Data.Should().BeNull();
        response.ErrorResult.Should().NotBeNull();
        response.ErrorResult.Should().BeEquivalentTo(errorContent);
    }
}
