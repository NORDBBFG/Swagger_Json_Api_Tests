using System;

public class PushNotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string Topic { get; set; }
    public DateTime? ScheduledTime { get; set; }
}