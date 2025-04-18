using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

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

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var invalidCategory = (QuickCommentCategory)999; // Invalid category

        // Act
        var response = await _apiClient.GetQuickCommentsAsync(invalidCategory);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        response.Data.Should().BeNull();
        response.ErrorResult.Should().NotBeNull();
        response.ErrorResult.StatusCode.Should().Be(400);
    }
}