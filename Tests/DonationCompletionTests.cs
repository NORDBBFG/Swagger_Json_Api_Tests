using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class DonationCompletionTests
{
    private DonationApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new DonationApiClient("https://api.example.com"); // Replace with actual base URL
    }

    [Test]
    public async Task AT_130_SuccessfulDonationCompletion()
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
            Status = 1, // Assuming 1 represents completed status
            BloodVolume = 450
        };

        // Act
        var response = await _client.CompleteDonationAsync(donation);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Id.Should().Be(donation.Id);
        response.Data.Status.Should().Be(1); // Completed status
        response.ErrorMessage.Should().BeNull();
    }

    [Test]
    public async Task AT_131_InvalidPayloadDonationCompletion()
    {
        // Arrange
        var invalidDonation = new DonationDto
        {
            // Missing required fields
            Id = Guid.Empty,
            BloodVolume = -1 // Invalid blood volume
        };

        // Act
        var response = await _client.CompleteDonationAsync(invalidDonation);

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().BeNull();
        response.ErrorMessage.Should().NotBeNullOrEmpty();
        response.ErrorMessage.Should().Contain("Bad request");
    }
}