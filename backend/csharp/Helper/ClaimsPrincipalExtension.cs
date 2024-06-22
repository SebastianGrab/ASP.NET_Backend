using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static List<long> GetOrganizationIds(this ClaimsPrincipal user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var orgaIdClaim = user.Claims
            .Where(c => c.Type == "Organization")
            .Select(c => long.Parse(c.Value))
            .ToList();

        return orgaIdClaim != null ? orgaIdClaim : throw new UnauthorizedAccessException("No Organization IDs found.");
    }

    public static long GetUserId(this ClaimsPrincipal user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "UserId");

        return userIdClaim != null ? long.Parse(userIdClaim.Value) : throw new UnauthorizedAccessException("User ID not found in claims.");
    }

    public static List<string> GetRoles(this ClaimsPrincipal user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        var roleClaim = user.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        return roleClaim != null ? roleClaim : throw new UnauthorizedAccessException("Role not found in claims.");
    }
}
