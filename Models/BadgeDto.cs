using System;

public class BadgeDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconUrl { get; set; }
}

public class ContentResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
}