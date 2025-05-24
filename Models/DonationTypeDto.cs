using System;

namespace DonorApp.Services.DTO
{
    public class DonationTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
