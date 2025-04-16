using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

[TestFixture]
public class BadgeTests
{
    private BadgeApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new BadgeApiClient(httpClient, "https://api.example.com");
    }

    [Test]
    public async Task AddBadge_SuccessfulRequest_ReturnsOkResponse()
    {
        var badge = new BadgeDto
        {
            Name = "Gold Donor",
            Description = "Awarded for donating over $1000",
            IconUrl = "https://example.com/icons/gold-donor.png"
        };

        var response = await _client.AddBadgeAsync(badge);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsTrue(response.Content.Success);
        Assert.AreEqual("Badge successfully added.", response.Content.Message);
        Assert.IsNotNull(response.Content.Data);
    }

    [Test]
    public async Task AddBadge_BadRequest_ReturnsBadRequestResponse()
    {
        var badge = new BadgeDto
        {
            Name = "",
            Description = "",
            IconUrl = ""
        };

        _client.SimulateError(true);
        var response = await _client.AddBadgeAsync(badge);

        Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.IsFalse(response.Content.Success);
        Assert.AreEqual("Simulated internal server error", response.Content.Message);
        Assert.IsNull(response.Content.Data);
    }

    [Test]
    public async Task EditBadge_SuccessfulRequest_ReturnsOkResponse()
    {
        var badge = new BadgeDto
        {
            Name = "Updated Gold Donor",
            Description = "Awarded for donating over $2000",
            IconUrl = "https://example.com/icons/updated-gold-donor.png"
        };

        var response = await _client.EditBadgeAsync(badge);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsTrue(response.Content.Success);
        Assert.AreEqual("Badge successfully edited.", response.Content.Message);
        Assert.IsNotNull(response.Content.Data);
    }

    [Test]
    public async Task EditBadge_BadRequest_ReturnsBadRequestResponse()
    {
        var badge = new BadgeDto
        {
            Name = "",
            Description = "",
            IconUrl = ""
        };

        _client.SimulateError(true);
        var response = await _client.EditBadgeAsync(badge);

        Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.IsFalse(response.Content.Success);
        Assert.AreEqual("Simulated internal server error", response.Content.Message);
        Assert.IsNull(response.Content.Data);
    }
}