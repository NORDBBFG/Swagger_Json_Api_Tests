using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using Moq;
using Moq.Protected;

[TestFixture]
public class ApiDnGet134Tests
{
    private ApiClient _apiClient;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;

    [SetUp]
    public void Setup()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var client = new HttpClient(_mockHttpMessageHandler.Object);
        _apiClient = new ApiClient(client);
    }

    [Test]
    public async Task GetBadge_ReturnsSuccessfulResponse()
    {
        // Arrange
        var expectedBadge = new BadgeDto
        {
            Id = 1,
            Name = "Test Badge",
            Description = "This is a test badge",
            Picture = "https://example.com/badge.png",
            CreateDate = DateTime.UtcNow.AddDays(-1),
            CreatedBy = "TestUser",
            UpdateDate = DateTime.UtcNow,
            LastUpdatedBy = "TestUser"
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(expectedBadge))
            });

        // Act
        var response = await _apiClient.GetBadgeAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedBadge);
    }

    [Test]
    public async Task GetBadge_ReturnsBadRequest()
    {
        // Arrange
        var errorContent = new ContentResult
        {
            Content = "Invalid request",
            ContentType = "application/json",
            StatusCode = 400
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(errorContent))
            });

        // Act
        var response = await _apiClient.GetBadgeAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Data.Should().BeNull();
        response.ErrorContent.Should().NotBeNull();
        response.ErrorContent.Should().BeEquivalentTo(errorContent);
    }

    [Test]
    public async Task GetBadge_ReturnsInternalServerError()
    {
        // Arrange
        var errorContent = new ContentResult
        {
            Content = "Internal server error",
            ContentType = "application/json",
            StatusCode = 500
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(errorContent))
            });

        // Act
        var response = await _apiClient.GetBadgeAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Data.Should().BeNull();
        response.ErrorContent.Should().NotBeNull();
        response.ErrorContent.Should().BeEquivalentTo(errorContent);
    }
}
