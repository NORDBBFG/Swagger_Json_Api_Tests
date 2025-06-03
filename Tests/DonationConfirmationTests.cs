using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class DonationConfirmationTests
{
    private DonationApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new DonationApiClient("https://api.example.com"); // Replace with actual base URL
    }

    [Test]
    public async Task ConfirmDonation_ValidData_ReturnsSuccessfulResponse()
    {
        // Arrange
        var donation = new DonationDto
        {
            Id = Guid.NewGuid(),
            UserId = "user123",
            FirstName = "John",
            LastName = "Doe",
            BloodType = "A+",
            DonationDate = DateTime.UtcNow,
            DonationType = "Whole Blood",
            Region = "Example Region",
            City = "Example City",
            DonationCenter = "Example Center",
            Status = (int)DonationStatuses.Pending,
            BloodVolume = 450
        };

        // Act
        var response = await _client.ConfirmDonationAsync(donation);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().NotBeNullOrEmpty();
        response.Data.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task ConfirmDonation_InvalidData_ReturnsBadRequest()
    {
        // Arrange
        var invalidDonation = new DonationDto
        {
            // Set invalid data, e.g., missing required fields
            Id = Guid.Empty,
            BloodVolume = -1
        };

        // Act
        var response = await _client.ConfirmDonationAsync(invalidDonation);

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().NotBeNullOrEmpty();
        response.Data.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task ConfirmDonation_ServerError_ReturnsInternalServerError()
    {
        // Arrange
        var donation = new DonationDto
        {
            // Set valid data that might trigger a server error
            Id = Guid.NewGuid(),
            UserId = "user_causing_server_error",
            // ... other properties
        };

        // Act
        var response = await _client.ConfirmDonationAsync(donation);

        // Assert
        response.StatusCode.Should().Be(500);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().NotBeNullOrEmpty();
        response.Data.StatusCode.Should().Be(500);
    }
}