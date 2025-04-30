using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Collections.Generic;

[TestFixture]
public class ApiTc001Tests
{
    private DonationCenterApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        // Add any necessary headers, like authorization
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer YOUR_API_KEY");
        _client = new DonationCenterApiClient(httpClient);
    }

    [Test]
    public async Task GetDonationCenters_ValidInput_ReturnsSuccessfulResponse()
    {
        // Arrange
        string donationCenterName = "Test Center";
        int regionId = 1;
        int cityId = 2;

        // Act
        var response = await _client.GetDonationCentersAsync(donationCenterName, regionId, cityId);

        // Assert
        response.StatusCode.Should().Be(200);
        response.IsSuccessful.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<List<DonationCenterDto>>();
        response.ErrorContent.Should().BeNull();

        // Additional assertions on the response data
        response.Data.Should().NotBeEmpty();
        response.Data.Should().AllSatisfy(center =>
        {
            center.Should().NotBeNull();
            center.Id.Should().BeGreaterThan(0);
            center.Name.Should().NotBeNullOrEmpty();
            center.RegionId.Should().BeGreaterThan(0);
            center.CityId.Should().BeGreaterThan(0);
            center.Address.Should().NotBeNullOrEmpty();
            center.PhoneNumber.Should().NotBeNullOrEmpty();
            center.Email.Should().NotBeNullOrEmpty();
            center.OpeningTime.Should().NotBe(default);
            center.ClosingTime.Should().NotBe(default);
        });
    }

    [Test]
    public async Task AddDonationCenter_ValidInput_ReturnsSuccessfulResponse()
    {
        // Arrange
        var newDonationCenter = new DonationCenterDto
        {
            Name = "New Test Center",
            RegionId = 1,
            CityId = 2,
            Address = "123 Test St",
            PhoneNumber = "123-456-7890",
            Email = "test@example.com",
            OpeningTime = new System.DateTime(2023, 1, 1, 8, 0, 0),
            ClosingTime = new System.DateTime(2023, 1, 1, 17, 0, 0)
        };

        // Act
        var response = await _client.AddDonationCenterAsync(newDonationCenter);

        // Assert
        response.StatusCode.Should().Be(200);
        response.IsSuccessful.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<DonationCenterDto>();
        response.ErrorContent.Should().BeNull();

        // Additional assertions on the response data
        response.Data.Id.Should().BeGreaterThan(0);
        response.Data.Name.Should().Be(newDonationCenter.Name);
        response.Data.RegionId.Should().Be(newDonationCenter.RegionId);
        response.Data.CityId.Should().Be(newDonationCenter.CityId);
        response.Data.Address.Should().Be(newDonationCenter.Address);
        response.Data.PhoneNumber.Should().Be(newDonationCenter.PhoneNumber);
        response.Data.Email.Should().Be(newDonationCenter.Email);
        response.Data.OpeningTime.Should().Be(newDonationCenter.OpeningTime);
        response.Data.ClosingTime.Should().Be(newDonationCenter.ClosingTime);
    }
}
