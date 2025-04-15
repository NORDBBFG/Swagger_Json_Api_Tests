using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

[TestFixture]
public class QuickCommentTests
{
    private QuickCommentApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        _apiClient = new QuickCommentApiClient("https://api.example.com");
    }

    [Test]
    public async Task GetQuickComments_ValidCategory_ReturnsOkWithComments()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _apiClient.GetQuickCommentsAsync(category);

        // Assert
        Assert.AreEqual(200, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.IsInstanceOf<List<QuickCommentDto>>(response.Data);
        Assert.IsTrue(response.Data.Count > 0);

        foreach (var comment in response.Data)
        {
            Assert.IsInstanceOf<int>(comment.Id);
            Assert.IsInstanceOf<string>(comment.Comment);
        }
    }

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var category = (QuickCommentCategory)999; // Invalid category

        // Act
        var response = await _apiClient.GetQuickCommentsAsync(category);

        // Assert
        Assert.AreEqual(400, response.StatusCode);
        Assert.IsNull(response.Data);
        Assert.IsNotNull(response.ErrorMessage);
    }

    [Test]
    public async Task GetQuickComments_ServerError_ReturnsInternalServerError()
    {
        // Arrange
        _apiClient.SimulateError(true);
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _apiClient.GetQuickCommentsAsync(category);

        // Assert
        Assert.AreEqual(500, response.StatusCode);
        Assert.IsNull(response.Data);
        Assert.IsNotNull(response.ErrorMessage);
    }
}