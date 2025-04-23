using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;

[TestFixture]
public class ApiTc001Tests
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
            quickComment.Id.Should().BePositive();
            quickComment.Comment.Should().BeOneOf(null, string.Empty).Or.BeAssignableTo<string>();
        }
    }

    [Test]
    public async Task GetQuickComments_ValidCategory_ReturnsValidStructure()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _client.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().AllBeOfType<QuickCommentDto>();
        response.Data.Should().OnlyContain(qc => qc.GetType().GetProperties().Select(p => p.Name).SequenceEqual(new[] { "Id", "Comment" }));
    }
}