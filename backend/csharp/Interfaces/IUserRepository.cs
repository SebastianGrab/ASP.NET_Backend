using Helper;
using Helper.SearchObjects;
using Models;

namespace Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers(QueryObject dateQuery, UserSearchObject userSearch);
        User GetUser(long id);
        User GetUserByEmail(string email);
        bool UserExists(long id);
        ICollection<User> GetUsersByOrganization(long organizationId, QueryObject dateQuery, UserSearchObject userSearch);
        User GetUserByProtocol(long protocolId);
        ICollection<User> GetAdditionalUsersByProtocol(long protocolId, QueryObject dateQuery, UserSearchObject userSearch);
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
        public bool VerifyUserPassword(User user, string password);
    }
}

