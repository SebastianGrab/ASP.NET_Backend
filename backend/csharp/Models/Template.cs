namespace Models
{
    public class Template : ITimestampEntity
    {
        public long Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public required string TemplateContent { get; set; }

        public long organizationId { get; set; }

        public Organization Organization { get; set; }

        public ICollection<TemplateOrganization> TemplateOrganizations { get; set; }

        public ICollection<Protocol> Protocols { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 
    }
}
