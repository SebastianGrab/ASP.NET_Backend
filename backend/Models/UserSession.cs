namespace Models
{
    public class UserSession : ITimestampEntity
    {
        public long Id { get; set; }

        public User User { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 
    }
}
