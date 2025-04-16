using System;
using System.Collections.Generic;

public class DonationFilter
{
    public string FullName { get; set; }
    public List<string> BloodType { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public List<int> DonationTypeIds { get; set; }
    public List<int> RegionIds { get; set; }
    public List<int> CityIds { get; set; }
    public List<int> DonationCenterIds { get; set; }
    public List<int> Status { get; set; }
    public List<int> BloodVolume { get; set; }
    public PageFilter Page { get; set; }
}

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

public class ContentResult
{
    public string Content { get; set; }
    public int StatusCode { get; set; }
}

public class PageFilter
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}