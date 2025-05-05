using System;
using System.Collections.Generic;

public class DonationCenterDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
    public List<string>? Phones { get; set; }
    public string? Comment { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastUpdatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public string? LastUpdatedByName { get; set; }
    public bool VisibleDays { get; set; }
    public List<string>? DutySchedule { get; set; }
    public List<WorkScheduleDto>? WorkSchedules { get; set; }
    public string? LinkOnTelegram { get; set; }
}

// Note: WorkScheduleDto is referenced but not defined in the provided schema.
// You may need to create this class separately if it's required.
public class WorkScheduleDto
{
    // Add properties as needed
}
