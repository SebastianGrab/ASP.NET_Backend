namespace Models
{
    public class User : ITimestampEntity
    {
        public long Id { get; set; }

        public required string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public DateTime LastPasswordChangeDate { get; set; }

        public bool PasswordChangeRequired {
            get {
                    return (DateTime.UtcNow - LastPasswordChangeDate).TotalDays > 365 || LastPasswordChangeDate.ToString("MM/dd/yyyy HH:mm:ss") == CreatedDate.ToString("MM/dd/yyyy HH:mm:ss"); 
                }
        }

        public ICollection<Protocol> Protocols { get; set; }
        
        public ICollection<AdditionalUser> AdditionalUser { get; set; }

        public ICollection<UserSession> UserSessions { get; set; }

        public ICollection<UserMessage> UserMessages { get; set; }

        public ICollection<UserOrganizationRole> UserOrganizationRoles { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 
    }
}
