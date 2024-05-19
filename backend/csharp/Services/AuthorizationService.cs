using System.Security.Claims;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

public class RoleRequirement : IAuthorizationRequirement
{
    public string Role { get; }

    public RoleRequirement(string role)
    {
        Role = role;
    }
}

public class RoleHandler : AuthorizationHandler<RoleRequirement>
{
    private readonly ProtocolContext _context;

    public RoleHandler(ProtocolContext context)
    {
        _context = context;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            context.Fail();
            return;
        }

        var userRoles = await _context.UserOrganizationRoles
            .Include(ur => ur.Role)
            .Include(ur => ur.Organization)
            .Where(ur => ur.userId == int.Parse(userId) && ur.Role.Name == requirement.Role)
            .ToListAsync();

        foreach (var userRole in userRoles)
        {
            if (HasAccessToOrganization(userRole.organizationId))
            {
                context.Succeed(requirement);
                return;
            }
        }

        context.Fail();
    }

    private bool HasAccessToOrganization(long organizationId)
    {
        // Implement logic to check if the user has access to the organization or its parents
        return true; // Simplified for demonstration
    }
}
