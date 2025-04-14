using NUnit.Framework;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DonorApp.Services.DTO;
using Newtonsoft.Json;

namespace DonorApp.Tests
{
    public class DonationConfirmTests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient { BaseAddress = new Uri("https://donorapp.org.ua/api/v1/") };
        }

        [Test]
        public async Task ConfirmDonation_ReturnsSuccess()
        {
            var donation = new DonationDto
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
                DonationStatus = DonationStatuses.Confirmed,
                BloodVolume = 500,
                HealthFeeling = HealthFeeling.Good,
                FeelingComment = "Feeling good",
                Certificates = null,
                ExperienceRate = 5,
                Comment = "Great experience",
                QuickComments = null
            };

            var content = new StringContent(JsonConvert.SerializeObject(donation), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("donation/confirm", content);

            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<ContentResult>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(200, result.StatusCode);
        }
    }
}
