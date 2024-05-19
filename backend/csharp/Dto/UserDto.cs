namespace Dto
{
    public class UserDto
    {
        public long Id { get; set; }

        public required string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public required string Email { get; set; }

        // public required string Password { get; set; }

        public DateTime LastPasswordChangeDate { get; set; }

        public bool PasswordChangeRequired { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 
    }
}
