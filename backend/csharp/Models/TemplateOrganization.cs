namespace Models
{
    public class TemplateOrganization
    {
        public long organizationId { get; set; }

        public Organization Organization { get; set; }

        public long templateId { get; set; }

        public Template Template { get; set; }
    }
}
