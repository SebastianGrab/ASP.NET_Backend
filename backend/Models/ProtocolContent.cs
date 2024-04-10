namespace Models
{
    public class ProtocolContent
    {
        public long Id { get; set; }

        public string Content { get; set; }

        public long protocolId { get; set; }

        public Protocol Protocol { get; set; }
    }
}
