namespace Models
{
    public class Protocol : ITimestampEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsDraft { get; set; }

        public string? ReviewComment { get; set; }

        public bool IsClosed { get; set; }

        public DateTime? ClosedAt { get; set; }

        public bool? sendEmail { get; set; }

        public string? emailSubject { get; set; }

        public string? emailContent { get; set; }

        public required User User { get; set; }

        public required Template Template { get; set; }
        
        public ProtocolContent ProtocolContent { get; set; }
        
        public ProtocolPdfFile ProtocolPdfFile { get; set; }
        
        public Organization Organization { get; set; }
        
        public ICollection<AdditionalUser> AdditionalUser { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 

    }
}