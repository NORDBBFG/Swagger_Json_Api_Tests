using NUnit.Framework;
using System.Collections.Generic;
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
        httpClient.BaseAddress = new System.Uri("https://api-base-url.com"); // Replace with actual base URL
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
            quickComment.Comment.Should().BeOfType<string>().Or.BeNull();
        }
    }

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var category = (QuickCommentCategory)999; // Invalid category

        // Act
        var response = await _client.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        response.Data.Should().BeNull();
        response.ErrorResult.Should().NotBeNull();
        response.ErrorResult.StatusCode.Should().Be(400);
    }
}