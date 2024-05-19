namespace Dto
{
    public class OrganizationDto
    {
        public long Id { get; set; }

        public long? parentId { get; set; }

        public required string Name { get; set; }

        public string? Address { get; set; }

        public string? PostalCode { get; set; }

        public string? City { get; set; }

        public required string OrganizationType { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 
    }
}