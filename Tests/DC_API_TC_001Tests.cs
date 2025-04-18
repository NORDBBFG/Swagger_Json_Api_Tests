using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using FluentAssertions;
using System.Net;

[TestFixture]
public class DC_API_TC_001Tests
{
    private DonationCenterApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new DonationCenterApiClient("https://api-base-url.com"); // Replace with actual base URL
    }

    [Test]
    public async Task GetDonationCenters_WithValidParameters_ReturnsSuccessfulResponse()
    {
        // Arrange
        string donationCenterName = "Red Cross";
        int regionId = 1;
        int cityId = 101;

        // Act
        var response = await _client.GetDonationCentersAsync(donationCenterName, regionId, cityId);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<DonationCenterDto>>();
        response.Data.Should().NotBeEmpty();

        foreach (var donationCenter in response.Data)
        {
            donationCenter.Name.Should().Contain(donationCenterName);
            donationCenter.RegionId.Should().Be(regionId);
            donationCenter.CityId.Should().Be(cityId);
        }
    }

    [Test]
    public async Task GetDonationCenters_ValidatesResponseContentType()
    {
        // Arrange
        string donationCenterName = "Red Cross";
        int regionId = 1;
        int cityId = 101;

        // Act
        var response = await _client.GetDonationCentersAsync(donationCenterName, regionId, cityId);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        // Note: We can't directly check the content type in this setup, 
        // but we can verify that the deserialization was successful
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<DonationCenterDto>>();
    }
}