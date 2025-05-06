using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiTc001Tests
{
    private DonationCenterApiClient _client;

    [SetUp]
    public void Setup()
    {
        string baseUrl = "https://api.example.com"; // Replace with actual base URL
        string bearerToken = "valid_token"; // Replace with actual token
        _client = new DonationCenterApiClient(baseUrl, bearerToken);
    }

    [Test]
    public async Task GetDonationCenters_ReturnsSuccessfulResponse()
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
        response.Data.Should().HaveCount(1);

        var donationCenter = response.Data.First();
        donationCenter.Should().NotBeNull();
        donationCenter.Id.Should().Be(1);
        donationCenter.Name.Should().Be("Sample Donation Center");
        donationCenter.Region.Should().Be("Sample Region");
        donationCenter.City.Should().Be("Sample City");
        donationCenter.Address.Should().Be("Sample Address");
        donationCenter.Phones.Should().ContainSingle().Which.Should().Be("123-456-7890");
        donationCenter.Comment.Should().Be("Sample Comment");
        donationCenter.Latitude.Should().BeApproximately(12.345678, 0.000001);
        donationCenter.Longitude.Should().BeApproximately(98.765432, 0.000001);
        donationCenter.CreatedDate.Should().Be(DateTime.Parse("2023-01-01T00:00:00Z"));
        donationCenter.UpdatedDate.Should().Be(DateTime.Parse("2023-01-02T00:00:00Z"));
        donationCenter.CreatedBy.Should().Be("Sample Creator");
        donationCenter.LastUpdatedBy.Should().Be("Sample Updater");
        donationCenter.CreatedByName.Should().Be("Sample Creator Name");
        donationCenter.LastUpdatedByName.Should().Be("Sample Updater Name");
        donationCenter.VisibleDays.Should().BeTrue();
        donationCenter.DutySchedule.Should().BeEquivalentTo(new[] { "Monday", "Tuesday" });
        donationCenter.WorkSchedules.Should().HaveCount(1);
        donationCenter.WorkSchedules[0].Day.Should().Be("Monday");
        donationCenter.WorkSchedules[0].StartTime.Should().Be("08:00");
        donationCenter.WorkSchedules[0].EndTime.Should().Be("17:00");
        donationCenter.LinkOnTelegram.Should().Be("https://t.me/sample");
    }
}