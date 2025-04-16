using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

[TestFixture]
public class BadgeTests
{
    private BadgeApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new BadgeApiClient("https://api.example.com");
    }

    [Test]
    public async Task GetBadges_ReturnsSuccessfully()
    {
        var response = await _client.GetBadgesAsync();

        Assert.AreEqual(200, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.IsInstanceOf<List<BadgeDto>>(response.Data);
    }

    [Test]
    public async Task GetBadges_ReturnsCorrectData()
    {
        var response = await _client.GetBadgesAsync();

        Assert.IsNotEmpty(response.Data);
        Assert.IsNull(response.ErrorMessage);

        var firstBadge = response.Data[0];
        Assert.IsNotNull(firstBadge.Name);
        Assert.IsNotNull(firstBadge.Description);
        Assert.Greater(firstBadge.Id, 0);
    }

    [Test]
    public async Task GetBadges_SimulateInternalServerError()
    {
        _client.SimulateError(true);
        var response = await _client.GetBadgesAsync();

        Assert.AreEqual(500, response.StatusCode);
        Assert.IsNull(response.Data);
        Assert.IsNotNull(response.ErrorMessage);
    }

    [Test]
    public async Task GetBadges_HandlesBadRequest()
    {
        _client = new BadgeApiClient("https://api.example.com/invalid");
        var response = await _client.GetBadgesAsync();

        Assert.AreEqual(400, response.StatusCode);
        Assert.IsNull(response.Data);
        Assert.IsNotNull(response.ErrorMessage);
    }
}