namespace Models
{
    public class UserMessage : ITimestampEntity
    {
        public long Id { get; set; }

        public string Subject { get; set; }

        public string MessageContent { get; set; }

        public string? ReferenceObject { get; set; }

        public long? ReferenceObjectId { get; set; }

        public DateTime SentAt { get; set; }

        public string? SentFrom { get; set; }

        public bool IsRead { get; set; }

        public bool IsArchived { get; set; }

        public User User { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 
    }
}
