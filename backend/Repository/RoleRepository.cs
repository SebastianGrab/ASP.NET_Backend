using Data;
using Interfaces;
using Models;

namespace Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ProtocolContext _context;
        
        public RoleRepository(ProtocolContext context)
        {
            _context = context;
        }

        public Role GetRole(long id)
        {
            return _context.Roles.Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Role> GetRoles()
        {
            return _context.Roles.OrderBy(r => r.Id).ToList();
        }

        public ICollection<Role> GetRolesByOrganization(long organizationId)
        {
            return _context.UserOrganizationRoles.Where(uor => uor.Organization.Id == organizationId).Select(r => r.Role).OrderBy(r => r.Id).ToList();
        }

        public ICollection<Role> GetRolesByUser(long userId)
        {
            return _context.UserOrganizationRoles.Where(uor => uor.User.Id == userId).Select(r => r.Role).OrderBy(r => r.Id).ToList();
        }

        public ICollection<Role> GetRolesByUserAndOrganization(long userId, long organizationId)
        {
            return _context.UserOrganizationRoles.Where(uor => uor.Organization.Id == organizationId && uor.User.Id == userId).Select(r => r.Role).OrderBy(r => r.Id).ToList();
        }

        public bool RoleExists(long id)
        {
            return _context.Roles.Any(r => r.Id == id);
        }
    }
}