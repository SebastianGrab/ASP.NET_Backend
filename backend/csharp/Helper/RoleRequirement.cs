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
