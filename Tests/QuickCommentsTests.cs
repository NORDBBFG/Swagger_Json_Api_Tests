using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

[TestFixture]
public class QuickCommentsTests
{
    private QuickCommentsApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        _apiClient = new QuickCommentsApiClient("https://api.example.com");
    }

    [Test]
    public async Task GetQuickComments_SuccessfulRequest_ReturnsOkWithComments()
    {
        var response = await _apiClient.GetQuickCommentsAsync(QuickCommentCategory.ReasonForCancellation);

        Assert.AreEqual(200, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.IsInstanceOf<List<QuickCommentDto>>(response.Data);
    }

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        var response = await _apiClient.GetQuickCommentsAsync((QuickCommentCategory)999);

        Assert.AreEqual(400, response.StatusCode);
        Assert.IsNull(response.Data);
        Assert.IsNotEmpty(response.ErrorMessage);
    }

    [Test]
    public async Task GetQuickComments_SimulatedServerError_ReturnsInternalServerError()
    {
        _apiClient.SimulateError(true);
        var response = await _apiClient.GetQuickCommentsAsync(QuickCommentCategory.ExperienceComment);

        Assert.AreEqual(500, response.StatusCode);
        Assert.IsNull(response.Data);
        Assert.IsNotEmpty(response.ErrorMessage);
    }
}