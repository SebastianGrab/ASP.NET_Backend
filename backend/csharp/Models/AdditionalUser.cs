namespace Models
{
    public class AdditionalUser
    {
        public long userId { get; set; }

        public required User User { get; set; }

        public long protocolId { get; set; }

        public required Protocol Protocol { get; set; }

    }
}
