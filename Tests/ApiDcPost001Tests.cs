using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiDcPost001Tests
{
    private DonationCenterApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new DonationCenterApiClient("https://api.example.com"); // Replace with actual base URL
    }

    [Test]
    public async Task AddDonationCenter_ValidData_ReturnsCreatedDonationCenter()
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
        response.Data.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        response.Data.UpdatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}