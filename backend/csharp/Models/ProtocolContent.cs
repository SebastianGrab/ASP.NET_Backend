namespace Models
{
    public class ProtocolContent : ITimestampEntity
    {
        public long Id { get; set; }

        public string Content { get; set; }

        public long protocolId { get; set; }

        public Protocol Protocol { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 
    }
}
