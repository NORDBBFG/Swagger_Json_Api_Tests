using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;

[TestFixture]
public class QuickCommentApiTests
{
    private QuickCommentApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new QuickCommentApiClient(httpClient);
    }

    [Test]
    public async Task GetQuickComments_ReasonForCancellation_ReturnsValidResponse()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _client.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().AllBeOfType<QuickCommentDto>();
        response.Data.Should().NotBeEmpty();
        response.Data.Should().OnlyContain(q => q.Id > 0);
        response.Data.Should().OnlyContain(q => !string.IsNullOrEmpty(q.Comment));
    }

    [Test]
    public async Task GetQuickComments_ExperienceComment_ReturnsValidResponse()
    {
        // Arrange
        var category = QuickCommentCategory.ExperienceComment;

        // Act
        var response = await _client.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().AllBeOfType<QuickCommentDto>();
        response.Data.Should().NotBeEmpty();
        response.Data.Should().OnlyContain(q => q.Id > 0);
        response.Data.Should().OnlyContain(q => !string.IsNullOrEmpty(q.Comment));
    }

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var category = (QuickCommentCategory)999; // Invalid category

        // Act
        var response = await _client.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().BeNull();
        response.ErrorMessage.Should().NotBeNullOrEmpty();
    }
}