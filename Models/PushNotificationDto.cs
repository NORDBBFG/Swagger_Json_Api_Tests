using System;

public class PushNotificationDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public int? TriggerId { get; set; }
    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }
    public DateTime NotificationDistribution { get; set; }
    public string CreatedById { get; set; }
    public string LastUpdatedById { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public bool IsDisabled { get; set; }
}