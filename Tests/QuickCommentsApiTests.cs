using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Collections.Generic;

[TestFixture]
public class QuickCommentsApiTests
{
    private QuickCommentsApiClient _client;
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer valid_token_12345");
        _client = new QuickCommentsApiClient(_httpClient);
    }

    [Test]
    public async Task GetQuickComments_ValidCategory_ReturnsSuccessfulResponse()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _client.GetQuickCommentsByCategoryAsync(category);

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<List<QuickCommentDto>>();
        response.Error.Should().BeNull();

        foreach (var comment in response.Data)
        {
            comment.Id.Should().BeGreaterThan(0);
            comment.Comment.Should().NotBeNullOrEmpty();
        }
    }

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var invalidCategory = (QuickCommentCategory)999; // Invalid category

        // Act
        var response = await _client.GetQuickCommentsByCategoryAsync(invalidCategory);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.Data.Should().BeNull();
        response.Error.Should().NotBeNull();
        response.Error.StatusCode.Should().Be(400);
        response.Error.Content.Should().NotBeNullOrEmpty();
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
}