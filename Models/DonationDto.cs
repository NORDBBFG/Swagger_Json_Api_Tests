using System;
using System.Collections.Generic;

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

public enum DonationStatuses
{
    Status1 = 1,
    Status2 = 2,
    Status3 = 3,
    Status4 = 4,
    Status5 = 5,
    Status6 = 6,
    Status7 = 7
}

public enum HealthFeeling
{
    Feeling1 = 1,
    Feeling2 = 2,
    Feeling3 = 3,
    Feeling4 = 4,
    Feeling5 = 5
}

public class CertificateDto
{
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public byte[] Picture { get; set; }
    public string PictureName { get; set; }
    public double PictureSize { get; set; }
    public CertificateStatus Status { get; set; }
}

public enum CertificateStatus
{
    Status1 = 1,
    Status2 = 2,
    Status3 = 3,
    Status4 = 4,
    Status5 = 5
}

public class QuickCommentDto
{
    public int Id { get; set; }
    public string Comment { get; set; }
}

public class ContentResult
{
    public string Content { get; set; }
    public string ContentType { get; set; }
    public int? StatusCode { get; set; }
}