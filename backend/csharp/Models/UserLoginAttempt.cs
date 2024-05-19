namespace Models
{
    public class UserLoginAttempt
    {
        public long Id { get; set; }

        public int FailedLoginAttempts { get; set; } = 0;
    
        public DateTime LastLoginAttempt { get; set; } = DateTime.UtcNow;

        public long userId { get; set; }

        public User User { get; set; }
    }
}
