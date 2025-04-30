using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;

[TestFixture]
public class ApiTc001Tests
{
    private QuickCommentApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _apiClient = new QuickCommentApiClient(httpClient);
    }

    [Test]
    public async Task GetQuickComments_SuccessfulRetrieval()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _apiClient.GetQuickCommentsByCategoryAsync(category);

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
    public async Task GetQuickComments_ValidateResponseStructure()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _apiClient.GetQuickCommentsByCategoryAsync(category);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<QuickCommentDto>>();

        response.Data.Should().AllSatisfy(quickComment =>
        {
            quickComment.Should().NotBeNull();
            quickComment.Id.Should().BeGreaterThan(0);
            quickComment.Comment.Should().BeOfType<string>().Or.BeNull();
        });
    }
}