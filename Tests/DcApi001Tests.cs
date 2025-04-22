using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class DcApi001Tests
{
    private DonationCenterApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new DonationCenterApiClient(httpClient);
    }

    [Test]
    public async Task GetDonationCenters_ReturnsSuccessfulResponse()
    {
        // Arrange
        // No additional arrangement needed as we're testing without query parameters

        // Act
        var response = await _client.GetDonationCentersAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull().And.NotBeEmpty();
        
        foreach (var donationCenter in response.Data)
        {
            donationCenter.Should().NotBeNull();
            donationCenter.Id.Should().BeGreaterThan(0);
            donationCenter.Name.Should().NotBeNullOrEmpty();
            donationCenter.RegionId.Should().BeGreaterThan(0);
            donationCenter.CityId.Should().BeGreaterThan(0);
            donationCenter.Address.Should().NotBeNullOrEmpty();
        }
    }
}