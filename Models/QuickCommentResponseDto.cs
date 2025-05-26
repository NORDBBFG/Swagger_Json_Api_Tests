using System;

namespace YourNamespace.Models
{
    public class QuickCommentResponseDto
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
