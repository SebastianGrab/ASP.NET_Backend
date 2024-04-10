using Models;

namespace Interfaces
{
    public interface IRoleRepository
    {
        ICollection<Role> GetRoles();
        Role GetRole(long id);
        bool RoleExists(long id);
        ICollection<Role> GetRolesByUser(long userId);
        ICollection<Role> GetRolesByOrganization(long organizationId);
        ICollection<Role> GetRolesByUserAndOrganization(long userId, long organizationId);
    }
}

