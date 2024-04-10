namespace Models
{
    public class ProtocolPdfFile
    {
        public long Id { get; set; }

        public string Content { get; set; }

        public string MimeType { get; set; }

        public long protocolId { get; set; }

        public Protocol Protocol { get; set; }
    }
}
