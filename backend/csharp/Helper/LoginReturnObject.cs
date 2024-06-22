using Models;

namespace Helper
{
    public class LoginReturnObject
    {
        public required string Token { get; set; }

        public long? userId { get; set; }

        public long? organizationId { get; set; }

        public string? role { get; set; }

        public List<UserOrganizationRole>? uor { get; set; }
    }
}