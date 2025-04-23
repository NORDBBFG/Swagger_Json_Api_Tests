using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiQc001Tests
{
    private QuickCommentsApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new QuickCommentsApiClient(httpClient);
    }

    [Test]
    public async Task GetQuickComments_ValidCategory_ReturnsQuickComments()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _client.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(200);
        response.IsSuccessStatusCode.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<QuickCommentDto>>();
        
        foreach (var comment in response.Data)
        {
            comment.Id.Should().BeGreaterThan(0);
            comment.Comment.Should().NotBeNull();
        }

        response.Headers.Should().ContainKey("Content-Type");
        response.Headers.GetValues("Content-Type").Should().Contain("application/json");
    }

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var invalidCategory = (QuickCommentCategory)0; // Invalid category

        // Act
        var response = await _client.GetQuickCommentsAsync(invalidCategory);

        // Assert
        response.StatusCode.Should().Be(400);
        response.IsSuccessStatusCode.Should().BeFalse();
        response.ErrorContent.Should().NotBeNullOrEmpty();
    }
}