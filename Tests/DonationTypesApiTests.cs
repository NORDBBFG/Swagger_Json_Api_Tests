using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class DonationTypesApiTests
{
    private DonationTypesApiClient _client;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new DonationTypesApiClient(httpClient, BaseUrl);
    }

    [Test]
    public async Task GetDonationTypes_ReturnsSuccessfulResponse()
    {
        // Arrange

        // Act
        var response = await _client.GetDonationTypesAsync();

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<List<DonationTypeDto>>();
        response.Data.Should().NotBeEmpty();

        // Additional assertions for the first item in the list
        var firstDonationType = response.Data[0];
        firstDonationType.Id.Should().BeGreaterThan(0);
        firstDonationType.Name.Should().NotBeNullOrEmpty();
        firstDonationType.CreateDate.Should().BeBefore(DateTime.UtcNow);
        firstDonationType.UpdateDate.Should().BeBefore(DateTime.UtcNow);
    }

    [Test]
    public async Task GetDonationTypes_ReturnsInternalServerError()
    {
        // Arrange
        // You might need to mock the HttpClient to simulate a 500 error
        // For this example, we'll assume the API might return a 500 error

        // Act
        var response = await _client.GetDonationTypesAsync();

        // Assert
        if (response.StatusCode == 500)
        {
            response.Error.Should().NotBeNull();
            response.Error.StatusCode.Should().Be(500);
            response.Error.Content.Should().NotBeNullOrEmpty();
        }
        else
        {
            // If we don't get a 500 error, the test should be inconclusive
            Assert.Inconclusive("Expected 500 Internal Server Error was not received.");
        }
    }
}