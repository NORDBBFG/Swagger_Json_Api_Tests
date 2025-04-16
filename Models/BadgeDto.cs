using System;

public class BadgeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Picture { get; set; }
    public DateTime CreateDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdateDate { get; set; }
    public string LastUpdatedBy { get; set; }
}

public class ContentResult
{
    public string Content { get; set; }
    public string ContentType { get; set; }
    public int? StatusCode { get; set; }
}