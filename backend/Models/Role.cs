namespace Models
{
    public class Role
    {
        public long Id { get; set; }

        public required string Name { get; set; }

        public ICollection<UserOrganizationRole> UserOrganizationRoles { get; set; }
    }
}
