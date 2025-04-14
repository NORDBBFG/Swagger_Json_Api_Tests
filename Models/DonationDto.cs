namespace DonorApp.Services.DTO
{
    public class DonationDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; }
        public string BloodType { get; set; }
        public int DonationTypeId { get; set; }
        public int CityId { get; set; }
        public int DonationCenterId { get; set; }
        public DonationStatuses DonationStatus { get; set; }
        public int BloodVolume { get; set; }
        public HealthFeeling HealthFeeling { get; set; }
        public string FeelingComment { get; set; }
        public List<CertificateDto> Certificates { get; set; }
        public int ExperienceRate { get; set; }
        public string Comment { get; set; }
        public List<QuickCommentDto> QuickComments { get; set; }
    }
}
