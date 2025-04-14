using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DonorApp.Services.ApiClient;
using DonorApp.Services.DTO;
using NUnit.Framework;

namespace DonorApp.Tests
{
    [TestFixture]
    public class DonationTests
    {
        private DonationApiClient _client;
        private const string BaseUrl = "https://api.example.com";

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            _client = new DonationApiClient(httpClient, BaseUrl);
        }

        [Test]
        public async Task ConfirmDonation_SuccessfulRequest_ReturnsOk()
        {
            // Arrange
            var donationDto = CreateSampleDonationDto();

            // Act
            var response = await _client.ConfirmDonationAsync(donationDto);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual("Donation successfully confirmed.", response.Content.Content);
        }

        [Test]
        public async Task ConfirmDonation_BadRequest_ReturnsBadRequest()
        {
            // Arrange
            var donationDto = new DonationDto(); // Invalid DTO

            // Act
            var response = await _client.ConfirmDonationAsync(donationDto);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual("Bad request! See json response for details.", response.Content.Content);
        }

        [Test]
        public async Task ConfirmDonation_ServerError_ReturnsInternalServerError()
        {
            // Arrange
            var donationDto = CreateSampleDonationDto();
            _client.SimulateError(true);

            // Act
            var response = await _client.ConfirmDonationAsync(donationDto);

            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual("Simulated server error", response.Content.Content);
        }

        private DonationDto CreateSampleDonationDto()
        {
            return new DonationDto
            {
                Id = Guid.NewGuid(),
                UserId = "user123",
                FirstName = "John",
                LastName = "Doe",
                Date = DateTime.UtcNow,
                BloodType = "A+",
                DonationTypeId = 1,
                CityId = 1,
                DonationCenterId = 1,
                DonationStatus = DonationStatuses.Status1,
                BloodVolume = 450,
                HealthFeeling = HealthFeeling.Feeling3,
                FeelingComment = "Feeling good",
                ExperienceRate = 5,
                Comment = "Great experience",
                Certificates = new List<CertificateDto>
                {
                    new CertificateDto
                    {
                        Id = Guid.NewGuid(),
                        CreateDate = DateTime.UtcNow,
                        PictureName = "certificate.jpg",
                        PictureSize = 1024,
                        Status = CertificateStatus.Status1
                    }
                },
                QuickComments = new List<QuickCommentDto>
                {
                    new QuickCommentDto { Id = 1, Comment = "Quick and easy" }
                }
            };
        }
    }
}
