using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;

[TestFixture]
public class TC001Tests
{
    private QuickCommentApiClient _apiClient;
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        _apiClient = new QuickCommentApiClient(_httpClient, "https://api.example.com"); // Replace with actual base URL
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
        response.Data.Should().BeOfType<List<QuickCommentDto>>();
        response.Data.Should().NotBeEmpty();
        response.ErrorResult.Should().BeNull();

        // Additional assertions
        response.Data.Should().AllSatisfy(comment =>
        {
            comment.Id.Should().BeGreaterThan(0);
            comment.Comment.Should().NotBeNullOrEmpty();
        });

        // Check response time
        // Note: This would require modifying the ApiClient to track response time
        // For demonstration purposes, we'll assume it's stored in a property of ApiResponse
        // response.ResponseTime.Should().BeLessThan(TimeSpan.FromSeconds(2));
    }

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var invalidCategory = (QuickCommentCategory)999; // Invalid category

        // Act
        var response = await _apiClient.GetQuickCommentsByCategoryAsync(invalidCategory);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        response.Data.Should().BeNull();
        response.ErrorResult.Should().NotBeNull();
        response.ErrorResult.StatusCode.Should().Be(400);
        response.ErrorResult.Content.Should().NotBeNullOrEmpty();
    }

    [TearDown]
    public void Teardown()
    {
        _httpClient.Dispose();
    }
}