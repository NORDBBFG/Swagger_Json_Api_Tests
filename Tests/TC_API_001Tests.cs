using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

[TestFixture]
public class TC_API_001Tests
{
    private HttpClient _httpClient;
    private UserApiClient _userApiClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api-base-url.com") // Replace with actual base URL
        };
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _userApiClient = new UserApiClient(_httpClient);
    }

    [Test]
    public async Task GetHolidays_ReturnsSuccessfulResponse()
    {
        // Act
        var response = await _userApiClient.GetHolidaysAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<Holiday>>();

        foreach (var holiday in response.Data)
        {
            holiday.Id.Should().BeGreaterThanOrEqualTo(0);
            holiday.Name.Should().NotBeNullOrEmpty();
            holiday.Date.Should().NotBe(default(DateTime));
        }
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
}