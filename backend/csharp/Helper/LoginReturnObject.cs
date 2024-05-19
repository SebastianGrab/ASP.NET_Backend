namespace Helper
{
    public class LoginReturnObject
    {
        public required string Token { get; set; }

        public long? userId { get; set; }

        public long? organizationId { get; set; }
    }
}