using System;

public class BadgeDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class ContentResult
{
    public string Content { get; set; }
    public int StatusCode { get; set; }
}