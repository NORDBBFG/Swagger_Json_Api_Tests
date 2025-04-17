using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class AT58Tests
{
    private QuickCommentsApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _apiClient = new QuickCommentsApiClient(httpClient);
    }

    [Test]
    public async Task GetQuickComments_ValidCategory_ReturnsSuccessfulResponse()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _apiClient.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<QuickCommentDto>>();
        response.Data.Should().NotBeEmpty();

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
        var response = await _apiClient.GetQuickCommentsAsync(invalidCategory);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.ErrorResult.Should().NotBeNull();
        response.ErrorResult.StatusCode.Should().Be(400);
        response.ErrorResult.Content.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task GetQuickComments_ServerError_ReturnsInternalServerError()
    {
        // Arrange
        // For this test, you might need to mock the HttpClient to simulate a 500 error
        // This is a placeholder for the concept

        // Act
        var response = await _apiClient.GetQuickCommentsAsync(QuickCommentCategory.ExperienceComment);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.ErrorResult.Should().NotBeNull();
        response.ErrorResult.StatusCode.Should().Be(500);
        response.ErrorResult.Content.Should().NotBeNullOrEmpty();
    }
}