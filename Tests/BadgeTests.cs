using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

[TestFixture]
public class BadgeTests
{
    private BadgeApiClient _client;
    private const string BaseUrl = "https://api.example.com";

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new BadgeApiClient(httpClient, BaseUrl);
    }

    [Test]
    public async Task GetBadges_ReturnsSuccessfulResponse()
    {
        var response = await _client.GetBadgesAsync();

        Assert.AreEqual(200, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.IsInstanceOf<List<BadgeDto>>(response.Data);
    }

    [Test]
    public async Task GetBadges_ReturnsCorrectDataStructure()
    {
        var response = await _client.GetBadgesAsync();

        Assert.AreEqual(200, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.IsInstanceOf<List<BadgeDto>>(response.Data);

        if (response.Data.Count > 0)
        {
            var firstBadge = response.Data[0];
            Assert.IsNotNull(firstBadge.Id);
            Assert.IsNotNull(firstBadge.Name);
            Assert.IsNotNull(firstBadge.CreateDate);
            Assert.IsNotNull(firstBadge.UpdateDate);
        }
    }

    [Test]
    public async Task GetBadges_SimulatedInternalServerError_ReturnsErrorResponse()
    {
        _client.SimulateError(true);
        var response = await _client.GetBadgesAsync();

        Assert.AreEqual(500, response.StatusCode);
        Assert.IsNull(response.Data);
        Assert.IsNotNull(response.ErrorMessage);
        Assert.AreEqual("Simulated Internal Server Error", response.ErrorMessage);
    }
}