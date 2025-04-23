using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;

[TestFixture]
public class ApiQcPost001Tests
{
    private HttpClient _httpClient;
    private QuickCommentApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new System.Uri("https://api-base-url.com"); // Replace with actual base URL
        _apiClient = new QuickCommentApiClient(_httpClient);
    }

    [Test]
    public async Task GetQuickComments_ValidCategory_ReturnsQuickComments()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _apiClient.GetQuickCommentsAsync(category);

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

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
}