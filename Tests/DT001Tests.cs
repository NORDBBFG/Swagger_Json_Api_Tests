using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Collections.Generic;

[TestFixture]
public class DT001Tests
{
    private DonationTypesApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new DonationTypesApiClient(httpClient);
    }

    [Test]
    public async Task GetDonationTypes_ShouldReturnSuccessfulResponse()
    {
        // Arrange
        // No additional arrangement needed as we're testing the GET endpoint

        // Act
        var response = await _client.GetDonationTypesAsync();

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().NotBeEmpty();

        foreach (var donationType in response.Data)
        {
            donationType.Should().NotBeNull();
            donationType.Id.Should().BeGreaterThan(0);
            donationType.Name.Should().NotBeNullOrEmpty();
        }
    }
}