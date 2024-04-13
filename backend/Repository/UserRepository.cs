using Data;
using Interfaces;
using Models;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ProtocolContext _context;
        
        public UserRepository(ProtocolContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool CreateUserOrganizationRole(UserOrganizationRole userOrganizationRole)
        {
            _context.Add(userOrganizationRole);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public bool DeleteUserOrganizationRole(UserOrganizationRole userOrganizationRole)
        {
            _context.Remove(userOrganizationRole);
            return Save();
        }

        public bool DeleteUserOrganizationRoles(List<UserOrganizationRole> userOrganizationRoles)
        {
            _context.RemoveRange(userOrganizationRoles);
            return Save();
        }

        public ICollection<User> GetAdditionalUsersByProtocol(long protocolId)
        {
            return _context.AdditionalUsers.Where(au => au.Protocol.Id == protocolId).Select(u => u.User).OrderByDescending(u => u.Id).ToList();
        }

        public ICollection<UserOrganizationRole> GetUserOrganizationRoleEntriesByUser(long id)
        {
            return _context.UserOrganizationRoles.Where(au => au.User.Id == id).ToList();
        }

        public User GetUser(long id)
        {
            return _context.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        public User GetUserByProtocol(long protocolId)
        {
            return _context.Protocols.Where(p => p.Id == protocolId).Select(u => u.User).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderByDescending(u => u.Id).ToList();
        }

        public ICollection<User> GetUsersByOrganization(long organizationId)
        {
            return _context.UserOrganizationRoles.Where(uor => uor.Organization.Id == organizationId).Select(u => u.User).OrderByDescending(u => u.Id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool UserExists(long id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        public bool UserOrganizationRoleExists(long userId, long organizationId, long roleId)
        {
            return _context.UserOrganizationRoles.Any(u => u.User.Id == userId && u.Organization.Id == organizationId && u.Role.Id == roleId);
        }

        public UserOrganizationRole GetUserOrganizationRole(long userId, long organizationId, long roleId)
        {
            return _context.UserOrganizationRoles.Where(u => u.User.Id == userId && u.Organization.Id == organizationId && u.Role.Id == roleId).FirstOrDefault();
        }
    }
}