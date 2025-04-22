using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiPostDonationCentersTests
{
    private DonationCenterApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new DonationCenterApiClient("https://api.example.com"); // Replace with your actual API base URL
    }

    [Test]
    public async Task AddDonationCenter_ValidData_ShouldCreateNewDonationCenter()
    {
        // Arrange
        var newDonationCenter = new DonationCenterDto
        {
            Name = "Test Donation Center",
            Region = "Test Region",
            City = "Test City",
            Address = "123 Test Street",
            Phones = new List<string> { "+1234567890" },
            Comment = "Test comment",
            Latitude = 40.7128,
            Longitude = -74.0060,
            VisibleDays = true,
            DutySchedule = new List<string> { "Monday", "Wednesday", "Friday" },
            WorkSchedules = new List<WorkScheduleDto>(), // Add sample work schedules if needed
            LinkOnTelegram = "https://t.me/testcenter"
        };

        // Act
        var response = await _client.AddDonationCenterAsync(newDonationCenter);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(newDonationCenter, options => options
            .Excluding(dc => dc.Id)
            .Excluding(dc => dc.CreatedDate)
            .Excluding(dc => dc.UpdatedDate)
            .Excluding(dc => dc.CreatedBy)
            .Excluding(dc => dc.LastUpdatedBy)
            .Excluding(dc => dc.CreatedByName)
            .Excluding(dc => dc.LastUpdatedByName));
        response.Data.Id.Should().BeGreaterThan(0);
        response.Data.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        response.Data.UpdatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
    }

    [Test]
    public async Task AddDonationCenter_InvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidDonationCenter = new DonationCenterDto
        {
            // Add invalid data, e.g., missing required fields
        };

        // Act
        var response = await _client.AddDonationCenterAsync(invalidDonationCenter);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        response.Data.Should().BeNull();
        response.ErrorContent.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task AddDonationCenter_ServerError_ShouldReturnInternalServerError()
    {
        // Arrange
        var newDonationCenter = new DonationCenterDto
        {
            // Add valid data that might trigger a server error
            Name = new string('A', 10000) // Assuming this might cause a server error
        };

        // Act
        var response = await _client.AddDonationCenterAsync(newDonationCenter);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
        response.Data.Should().BeNull();
        response.ErrorContent.Should().NotBeNullOrEmpty();
    }
}