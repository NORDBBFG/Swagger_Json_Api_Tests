using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;

[TestFixture]
public class ApiDnGet001Tests
{
    private DonationCenterApiClient _client;

    [SetUp]
    public void Setup()
    {
        string baseUrl = "https://api.example.com"; // Replace with actual base URL
        string token = "your_valid_token_here"; // Replace with actual token
        _client = new DonationCenterApiClient(baseUrl, token);
    }

    [Test]
    public async Task GetDonationCenters_WithValidParameters_ReturnsSuccessfulResponse()
    {
        // Arrange
        string donationCenterName = "Sample Donation Center";
        int regionId = 1;
        int cityId = 1;

        // Act
        var response = await _client.GetDonationCentersAsync(donationCenterName, regionId, cityId);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<DonationCenterDto>>();

        if (response.Data.Any())
        {
            var firstCenter = response.Data.First();
            firstCenter.Should().NotBeNull();
            firstCenter.Id.Should().BeGreaterThan(0);
            firstCenter.Name.Should().NotBeNullOrEmpty();
            firstCenter.Region.Should().NotBeNullOrEmpty();
            firstCenter.City.Should().NotBeNullOrEmpty();
            firstCenter.Address.Should().NotBeNullOrEmpty();
            firstCenter.Phones.Should().NotBeNull();
            firstCenter.Latitude.Should().BeInRange(-90, 90);
            firstCenter.Longitude.Should().BeInRange(-180, 180);
            firstCenter.CreatedDate.Should().BeBefore(System.DateTime.UtcNow);
            firstCenter.UpdatedDate.Should().BeBefore(System.DateTime.UtcNow);
            firstCenter.CreatedBy.Should().NotBeNullOrEmpty();
            firstCenter.LastUpdatedBy.Should().NotBeNullOrEmpty();
            firstCenter.CreatedByName.Should().NotBeNullOrEmpty();
            firstCenter.LastUpdatedByName.Should().NotBeNullOrEmpty();
            firstCenter.DutySchedule.Should().NotBeNull();
            firstCenter.WorkSchedules.Should().NotBeNull();
        }
    }
}