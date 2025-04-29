using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiGetDc001Tests
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
        string donationCenterName = "Red Cross Center";
        int regionId = 1;
        int cityId = 101;

        // Act
        var response = await _client.GetDonationCentersAsync(donationCenterName, regionId, cityId);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<System.Collections.Generic.List<DonationCenterDto>>();

        if (response.Data.Any())
        {
            var firstDonationCenter = response.Data.First();
            firstDonationCenter.Should().NotBeNull();
            firstDonationCenter.Id.Should().BeOfType(typeof(int));
            firstDonationCenter.Name.Should().BeOfType(typeof(string));
            firstDonationCenter.Region.Should().BeOfType(typeof(string));
            firstDonationCenter.City.Should().BeOfType(typeof(string));
            firstDonationCenter.Address.Should().BeOfType(typeof(string));
            firstDonationCenter.Phones.Should().BeAssignableTo<System.Collections.Generic.List<string>>();
            firstDonationCenter.Comment.Should().BeOfType(typeof(string));
            firstDonationCenter.Latitude.Should().BeOfType(typeof(double));
            firstDonationCenter.Longitude.Should().BeOfType(typeof(double));
            firstDonationCenter.CreatedDate.Should().BeOfType(typeof(DateTime));
            firstDonationCenter.UpdatedDate.Should().BeOfType(typeof(DateTime));
            firstDonationCenter.CreatedBy.Should().BeOfType(typeof(string));
            firstDonationCenter.LastUpdatedBy.Should().BeOfType(typeof(string));
            firstDonationCenter.CreatedByName.Should().BeOfType(typeof(string));
            firstDonationCenter.LastUpdatedByName.Should().BeOfType(typeof(string));
            firstDonationCenter.VisibleDays.Should().BeOfType(typeof(bool));
            firstDonationCenter.DutySchedule.Should().BeAssignableTo<System.Collections.Generic.List<string>>();
            firstDonationCenter.WorkSchedules.Should().BeAssignableTo<System.Collections.Generic.List<WorkScheduleDto>>();
            firstDonationCenter.LinkOnTelegram.Should().BeOfType(typeof(string));
        }
    }
}