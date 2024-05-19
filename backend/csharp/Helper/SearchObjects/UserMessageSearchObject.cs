namespace Helper.SearchObjects
{
    public class UserMessageSearchObject
    {
        public string? Subject { get; set; }

        public string? MessageContent { get; set; } 

        public string? SentFrom { get; set; }

        public bool? IsRead { get; set; }

        public bool? IsArchived { get; set; }
    }
}