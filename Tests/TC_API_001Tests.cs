using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class TC_API_001Tests
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
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<System.Collections.Generic.List<DonationCenterDto>>();

        foreach (var donationCenter in response.Data)
        {
            donationCenter.Id.Should().BeGreaterThan(0);
            donationCenter.Name.Should().NotBeNullOrEmpty();
            donationCenter.Region.Should().NotBeNullOrEmpty();
            donationCenter.City.Should().NotBeNullOrEmpty();
            donationCenter.Address.Should().NotBeNullOrEmpty();
            donationCenter.Phones.Should().NotBeNull();
            donationCenter.Latitude.Should().BeInRange(-90, 90);
            donationCenter.Longitude.Should().BeInRange(-180, 180);
            donationCenter.CreatedDate.Should().BeAfter(DateTime.MinValue);
            donationCenter.UpdatedDate.Should().BeAfter(DateTime.MinValue);
        }
    }
}