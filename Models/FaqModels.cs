using System;
using System.Collections.Generic;

public class FaqFilter
{
    public int CategoryId { get; set; }
    public bool? IsDraft { get; set; }
    public string CreatedBy { get; set; }
    public string Search { get; set; }
}

public class FaqArticleDto
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public string Picture { get; set; }
    public bool IsDraft { get; set; }
    public bool IsPermanent { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public FaqCategoryDto Category { get; set; }
    public List<RelatedFaqArticleDto> RelatedQuestions { get; set; }
    public string CreatedBy { get; set; }
    public string LastUpdatedBy { get; set; }
    public string CreatedByName { get; set; }
    public string LastUpdatedByName { get; set; }
}

public class ContentResult
{
    public string Content { get; set; }
    public string ContentType { get; set; }
    public int? StatusCode { get; set; }
}

public class FaqCategoryDto
{
}

public class RelatedFaqArticleDto
{
}