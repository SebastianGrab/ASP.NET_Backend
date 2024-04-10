namespace Models
{
    public class UserOrganizationRole : ITimestampEntity
    {
        public long Id { get; set;}
        
        public long organizationId { get; set; }

        public Organization Organization { get; set; }

        public long userId { get; set; }

        public User User { get; set; }

        public long roleId { get; set; }

        public Role Role { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 
    }
}
