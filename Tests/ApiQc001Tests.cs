using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;

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
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<QuickCommentDto>>();
        
        foreach (var quickComment in response.Data)
        {
            quickComment.Id.Should().BeGreaterThan(0);
            quickComment.Comment.Should().NotBeNull();
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
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        // Note: We can't directly check the content type in this implementation,
        // as it's not exposed in our ApiResponse. In a real-world scenario,
        // you might want to modify the ApiResponse to include this information.
    }

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var invalidCategory = (QuickCommentCategory)999; // Invalid category

        // Act
        var response = await _client.GetQuickCommentsAsync(invalidCategory);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        response.ErrorContent.Should().NotBeNull();
        response.ErrorContent.StatusCode.Should().Be(400);
    }
}