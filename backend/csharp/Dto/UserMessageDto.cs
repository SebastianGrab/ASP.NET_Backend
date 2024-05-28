namespace Dto
{
    public class UserMessageDto
    {
        public long Id { get; set; }

        public string Subject { get; set; }

        public string MessageContent { get; set; }

        public DateTime SentAt { get; set; }

        public string SentFrom { get; set; }

        public bool IsRead { get; set; }

        public bool IsArchived { get; set; }

        public long userId { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 
    }
}
