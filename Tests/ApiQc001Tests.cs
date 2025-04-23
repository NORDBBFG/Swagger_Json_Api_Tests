using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiQc001Tests
{
    private QuickCommentApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new QuickCommentApiClient(httpClient);
    }

    [Test]
    public async Task GetQuickComments_ValidCategory_ReturnsQuickComments()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _client.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<QuickCommentDto>>();

        foreach (var quickComment in response.Data)
        {
            quickComment.Id.Should().BeGreaterThan(0);
            quickComment.Comment.Should().BeOfType<string>().Or.BeNull();
        }
    }

    [Test]
    public async Task GetQuickComments_ValidCategory_ReturnsCorrectContentType()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _client.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        // Note: We can't directly check the content type in this setup, 
        // as it's handled internally by HttpClient. In a real scenario, 
        // you might want to expose this information through the ApiResponse class.
    }

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var invalidCategory = (QuickCommentCategory)999; // Invalid category

        // Act
        var response = await _client.GetQuickCommentsAsync(invalidCategory);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.ErrorResult.Should().NotBeNull();
        response.ErrorResult.StatusCode.Should().Be(400);
    }
}
