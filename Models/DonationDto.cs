public class DonationDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BloodType { get; set; }
    public DateTime DonationDate { get; set; }
    public string DonationType { get; set; }
    public string Region { get; set; }
    public string City { get; set; }
    public string DonationCenter { get; set; }
    public int Status { get; set; }
    public int BloodVolume { get; set; }
}