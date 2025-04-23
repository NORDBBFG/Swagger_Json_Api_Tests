using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Collections.Generic;

[TestFixture]
public class TC001QuickCommentsTests
{
    private QuickCommentApiClient _apiClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _apiClient = new QuickCommentApiClient(httpClient, BaseUrl);
    }

    [Test]
    public async Task GetQuickComments_ValidCategory_ReturnsSuccessfulResponse()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _apiClient.GetQuickCommentsByCategoryAsync(category);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<QuickCommentDto>>();
        response.Data.Should().NotBeEmpty();

        foreach (var comment in response.Data)
        {
            comment.Id.Should().BeGreaterThan(0);
            comment.Comment.Should().NotBeNullOrEmpty();
        }

        response.ErrorResult.Should().BeNull();
    }

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var invalidCategory = (QuickCommentCategory)99; // Invalid category

        // Act
        var response = await _apiClient.GetQuickCommentsByCategoryAsync(invalidCategory);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        response.Data.Should().BeNull();
        response.ErrorResult.Should().NotBeNull();
        response.ErrorResult.StatusCode.Should().Be(400);
        response.ErrorResult.Content.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task GetQuickComments_ValidCategory_ResponseTimeWithinLimit()
    {
        // Arrange
        var category = QuickCommentCategory.ExperienceComment;
        var maxResponseTime = TimeSpan.FromSeconds(2);

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _apiClient.GetQuickCommentsByCategoryAsync(category);
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        stopwatch.Elapsed.Should().BeLessThan(maxResponseTime);
    }
}