namespace Models
{
    public class Organization : ITimestampEntity
    {
        public long Id { get; set; }

        public long? parentId { get; set; }

        public required string Name { get; set; }

        public string? Address { get; set; }

        public string? PostalCode { get; set; }

        public string? City { get; set; }

        public required string OrganizationType { get; set; }

        public ICollection<TemplateOrganization> TemplateOrganizations { get; set; }

        public ICollection<UserOrganizationRole> UserOrganizationRoles { get; set; }

        public ICollection<Protocol> Protocols { get; set; }

        public ICollection<Template> Templates { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 
    }
}