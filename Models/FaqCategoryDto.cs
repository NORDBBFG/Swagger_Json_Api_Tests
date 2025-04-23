using System;

namespace YourNamespace.Models
{
    public class FaqCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public int FaqArticlesCount { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string LastUpdatedByName { get; set; }
        public bool IsPermanent { get; set; }
    }
}