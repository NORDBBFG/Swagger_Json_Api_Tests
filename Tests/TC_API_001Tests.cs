using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using FluentAssertions;

[TestFixture]
public class TC_API_001Tests
{
    private DonationCenterApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new DonationCenterApiClient("https://api.example.com"); // Replace with actual base URL
    }

    [Test]
    public async Task GetDonationCenters_WithValidParameters_ReturnsMatchingDonationCenters()
    {
        // Arrange
        string donationCenterName = "Red Cross";
        int regionId = 1;
        int cityId = 101;

        // Act
        var response = await _client.GetDonationCentersAsync(donationCenterName, regionId, cityId);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        Assert.That(response.Data, Is.Not.Null);
        Assert.That(response.Data, Is.Not.Empty);

        foreach (var donationCenter in response.Data)
        {
            donationCenter.Should().NotBeNull();
            donationCenter.Should().BeOfType<DonationCenterDto>();
            donationCenter.Name.Should().Be(donationCenterName);
            // Note: regionId and cityId are not directly accessible in the DonationCenterDto
            // You may need to adjust these assertions based on how region and city are represented
            donationCenter.Region.Should().NotBeNullOrEmpty();
            donationCenter.City.Should().NotBeNullOrEmpty();
        }
    }
}