using System;
using System.Collections.Generic;

namespace YourNamespace.Models
{
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
}