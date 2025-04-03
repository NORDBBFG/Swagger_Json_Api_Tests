using System;

namespace RestApiNUnitTests.Models
{
    public class DonationTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string IconData { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public string HeaderColor { get; set; }
    }
}