public enum QuickCommentCategory
{
    ReasonsForCancellation = 1,
    ExperienceOfDonation = 2
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
