using Models;

namespace Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(long id);
        bool UserExists(long id);
        ICollection<User> GetUsersByOrganization(long organizationId);
        User GetUserByProtocol(long protocolId);
        ICollection<User> GetAdditionalUsersByProtocol(long protocolId);
        ICollection<UserOrganizationRole> GetUserOrganizationRoleEntriesByUser(long id);
        UserOrganizationRole GetUserOrganizationRole(long userId, long organizationId, long roleId);
        bool CreateUser(User user);
        bool CreateUserOrganizationRole(UserOrganizationRole userOrganizationRole);
        bool DeleteUserOrganizationRole(UserOrganizationRole userOrganizationRole);
        bool DeleteUserOrganizationRoles(List<UserOrganizationRole> userOrganizationRoles);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
        bool UserOrganizationRoleExists(long userId, long organizationId, long roleId);
    }
}

