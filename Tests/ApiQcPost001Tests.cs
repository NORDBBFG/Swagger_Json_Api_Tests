using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;

[TestFixture]
public class ApiQcPost001Tests
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
    public async Task GetQuickComments_ValidCategory_ReturnsCorrectStructure()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _client.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
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