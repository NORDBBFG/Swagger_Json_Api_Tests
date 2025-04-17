using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

[TestFixture]
public class QuickCommentTests
{
    private QuickCommentApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new QuickCommentApiClient("http://api.example.com");
    }

    [Test]
    public async Task GetQuickComments_SuccessfulRequest_ReturnsOkWithData()
    {
        var response = await _client.GetQuickCommentsAsync(QuickCommentCategory.ReasonsForCancellation);

        Assert.AreEqual(200, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.IsInstanceOf<List<QuickCommentDto>>(response.Data);
    }

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        _client.SimulateError(true);
        var response = await _client.GetQuickCommentsAsync((QuickCommentCategory)999);

        Assert.AreEqual(400, response.StatusCode);
        Assert.IsNull(response.Data);
        Assert.IsNotNull(response.ErrorMessage);
    }

    [Test]
    public async Task GetQuickComments_ServerError_ReturnsInternalServerError()
    {
        _client.SimulateError(true);
        var response = await _client.GetQuickCommentsAsync(QuickCommentCategory.ExperienceOfDonation);

        Assert.AreEqual(500, response.StatusCode);
        Assert.IsNull(response.Data);
        Assert.IsNotNull(response.ErrorMessage);
    }
}
