using System;

public class PushNotificationCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Picture { get; set; }
    public int PushNotificationsCount { get; set; }
    public string CreatedById { get; set; }
    public string LastUpdatedById { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public bool IsDisabled { get; set; }
}