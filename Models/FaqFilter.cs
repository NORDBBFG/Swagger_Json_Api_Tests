using System;

namespace YourNamespace.Models
{
    public class FaqFilter
    {
        public int CategoryId { get; set; }
        public bool? IsDraft { get; set; }
        public string CreatedBy { get; set; }
        public string Search { get; set; }
    }
}