namespace Dto
{
    public class ProtocolPdfFileDto
    {
        public long protocolId { get; set; } 
        
        public string Content { get; set; }

        public string MimeType { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 
    }
}
