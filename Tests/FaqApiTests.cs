using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

[TestFixture]
public class FaqApiTests
{
    private FaqApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new FaqApiClient("http://api.example.com");
    }

    [Test]
    public async Task FilterFaqs_SuccessfulRequest_ReturnsOkWithFaqArticles()
    {
        var filter = new FaqFilter
        {
            CategoryId = 1,
            IsDraft = false,
            CreatedBy = "John Doe",
            Search = "test"
        };

        var response = await _client.FilterFaqsAsync(filter);

        Assert.AreEqual(200, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.IsInstanceOf<List<FaqArticleDto>>(response.Data);
        Assert.IsNull(response.ErrorMessage);
    }

    [Test]
    public async Task FilterFaqs_EmptyFilter_ReturnsOkWithAllFaqArticles()
    {
        var filter = new FaqFilter();

        var response = await _client.FilterFaqsAsync(filter);

        Assert.AreEqual(200, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.IsInstanceOf<List<FaqArticleDto>>(response.Data);
        Assert.IsNull(response.ErrorMessage);
    }

    [Test]
    public async Task FilterFaqs_InternalServerError_Returns500WithErrorMessage()
    {
        _client.SimulateError(true);

        var filter = new FaqFilter();

        var response = await _client.FilterFaqsAsync(filter);

        Assert.AreEqual(500, response.StatusCode);
        Assert.IsNull(response.Data);
        Assert.IsNotNull(response.ErrorMessage);
        Assert.AreEqual("Simulated internal server error", response.ErrorMessage);
    }
}