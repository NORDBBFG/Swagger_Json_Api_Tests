using System;

namespace YourNamespace.Models
{
    public class QuickCommentDto
    {
        public long Id { get; set; }
        public string Comment { get; set; }
        public string Author { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
