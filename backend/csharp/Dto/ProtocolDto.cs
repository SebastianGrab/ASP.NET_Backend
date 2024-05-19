namespace Dto
{
    public class ProtocolDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsDraft { get; set; }

        public string? ReviewComment { get; set; }

        public bool IsClosed { get; set; }

        public DateTime? ClosedAt { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 
    }
}