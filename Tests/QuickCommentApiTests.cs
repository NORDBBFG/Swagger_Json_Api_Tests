using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class QuickCommentApiTests
{
    private QuickCommentApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new QuickCommentApiClient("https://api-base-url.com");
    }

    [Test]
    public async Task GetQuickComments_ReasonForCancellation_ReturnsSuccessfulResponse()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _client.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<List<QuickCommentDto>>();
        response.Data.Should().NotBeEmpty();
        response.Data.ForEach(comment =>
        {
            comment.Id.Should().BeGreaterThan(0);
            comment.Comment.Should().NotBeNullOrEmpty();
        });
    }

    [Test]
    public async Task GetQuickComments_ExperienceOfDonation_ReturnsSuccessfulResponse()
    {
        // Arrange
        var category = QuickCommentCategory.ExperienceOfDonation;

        // Act
        var response = await _client.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<List<QuickCommentDto>>();
        response.Data.Should().NotBeEmpty();
        response.Data.ForEach(comment =>
        {
            comment.Id.Should().BeGreaterThan(0);
            comment.Comment.Should().NotBeNullOrEmpty();
        });
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