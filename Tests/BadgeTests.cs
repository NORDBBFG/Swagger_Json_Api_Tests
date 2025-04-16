using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

[TestFixture]
public class BadgeTests
{
    private BadgeApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new BadgeApiClient("http://api.example.com");
    }

    [Test]
    public async Task AddBadge_SuccessfulRequest_ReturnsOk()
    {
        var badge = new BadgeDto { Name = "Test Badge", Description = "Test Description" };

        var response = await _client.AddBadgeAsync(badge);

        Assert.AreEqual(200, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(200, response.Data.StatusCode);
    }

    [Test]
    public async Task AddBadge_BadRequest_ReturnsBadRequest()
    {
        var badge = new BadgeDto { Description = "Test Description" };

        var response = await _client.AddBadgeAsync(badge);

        Assert.AreEqual(400, response.StatusCode);
        Assert.IsNotNull(response.ErrorMessage);
    }

    [Test]
    public async Task AddBadge_ServerError_ReturnsInternalServerError()
    {
        _client.SimulateError(true);
        var badge = new BadgeDto { Name = "Test Badge", Description = "Test Description" };

        var response = await _client.AddBadgeAsync(badge);

        Assert.AreEqual(500, response.StatusCode);
        Assert.IsNotNull(response.ErrorMessage);
    }

    [Test]
    public async Task EditBadge_SuccessfulRequest_ReturnsOk()
    {
        var badge = new BadgeDto { Id = 1, Name = "Updated Badge", Description = "Updated Description" };

        var response = await _client.EditBadgeAsync(badge);

        Assert.AreEqual(200, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(200, response.Data.StatusCode);
    }

    [Test]
    public async Task EditBadge_BadRequest_ReturnsBadRequest()
    {
        var badge = new BadgeDto { Id = 1, Description = "Updated Description" };

        var response = await _client.EditBadgeAsync(badge);

        Assert.AreEqual(400, response.StatusCode);
        Assert.IsNotNull(response.ErrorMessage);
    }

    [Test]
    public async Task EditBadge_ServerError_ReturnsInternalServerError()
    {
        _client.SimulateError(true);
        var badge = new BadgeDto { Id = 1, Name = "Updated Badge", Description = "Updated Description" };

        var response = await _client.EditBadgeAsync(badge);

        Assert.AreEqual(500, response.StatusCode);
        Assert.IsNotNull(response.ErrorMessage);
    }
}