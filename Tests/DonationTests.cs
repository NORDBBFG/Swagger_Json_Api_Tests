using NUnit.Framework;\nusing System;\nusing System.Net;\nusing System.Threading.Tasks;\n\n[TestFixture]\npublic class DonationTests\n{\n    private DonationApiClient _client;\n\n    [SetUp]\n    public void Setup()\n    {\n        _client = new DonationApiClient(\"https://api.example.com\");\n    }\n\n    [Test]\n    public async Task ConfirmDonation_SuccessfulRequest_ReturnsOk()\n    {\n        // Arrange\n        var donationDto = new DonationDto\n        {\n            Id = Guid.NewGuid(),\n            UserId = \"user123\",\n            FirstName = \"John\",\n            LastName = \"Doe\",\n            Date = DateTime.UtcNow,\n            BloodType = \"A+\",\n            DonationTypeId = 1,\n            CityId = 1,\n            DonationCenterId = 1,\n            DonationStatus = DonationStatuses.Status1,\n            BloodVolume = 450,\n            HealthFeeling = HealthFeeling.Feeling1,\n            FeelingComment = \"Feeling good\",\n            ExperienceRate = 5,\n            Comment = \"Great experience\"\n        };\n\n        // Act\n        var response = await _client.ConfirmDonationAsync(donationDto);\n\n        // Assert\n        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);\n        Assert.IsNotNull(response.Content);\n        Assert.AreEqual(200, response.Content.StatusCode);\n    }\n\n    [Test]\n    public async Task ConfirmDonation_BadRequest_ReturnsBadRequest()\n    {\n        // Arrange\n        var donationDto = new DonationDto(); // Invalid DTO\n\n        // Act\n        var response = await _client.ConfirmDonationAsync(donationDto);\n\n        // Assert\n        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);\n        Assert.IsNotNull(response.Content);\n        Assert.AreEqual(400, response.Content.StatusCode);\n    }\n\n    [Test]\n    public async Task ConfirmDonation_ServerError_ReturnsInternalServerError()\n    {\n        // Arrange\n        var donationDto = new DonationDto\n        {\n            Id = Guid.NewGuid(),\n            UserId = \"user123\",\n            FirstName = \"John\",\n            LastName = \"Doe\",\n            Date = DateTime.UtcNow,\n            BloodType = \"A+\",\n            DonationTypeId = 1,\n            CityId = 1,\n            DonationCenterId = 1,\n            DonationStatus = DonationStatuses.Status1,\n            BloodVolume = 450,\n            HealthFeeling = HealthFeeling.Feeling1,\n            FeelingComment = \"Feeling good\",\n            ExperienceRate = 5,\n            Comment = \"Great experience\"\n        };\n\n        _client.SimulateError(true);\n\n        // Act\n        var response = await _client.ConfirmDonationAsync(donationDto);\n\n        // Assert\n        Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);\n        Assert.IsNotNull(response.Content);\n        Assert.AreEqual(500, response.Content.StatusCode);\n    }\n}