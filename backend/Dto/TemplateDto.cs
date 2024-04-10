namespace Dto
{
    public class TemplateDto
    {
        public long Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public required string TemplateContent { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 
    }
}
