using Data;
using Interfaces;
using Models;

namespace Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ProtocolContext _context;
        
        public OrganizationRepository(ProtocolContext context)
        {
            _context = context;
        }

        public ICollection<Organization> GetOrganizations()
        {
            return _context.Organizations.OrderByDescending(o => o.Id).ToList();
        }

        public Organization GetOrganization(long id)
        {
            return _context.Organizations.Where(o => o.Id == id).FirstOrDefault();
        }

        public bool OrganizationExists(long id)
        {
            return _context.Organizations.Any(o => o.Id == id);
        }

        public ICollection<Organization> GetOrganizationsByUser(long userId)
        {
            return _context.UserOrganizationRoles.Where(uor => uor.User.Id == userId).Select(o => o.Organization).ToList();
        }

        public ICollection<Organization> GetOrganizationsByTemplate(long templateId)
        {
            return _context.TemplateOrganizations.Where(to => to.Template.Id == templateId).Select(o => o.Organization).ToList();
        }

        public Organization GetOrganizationByProtocol(long protocolId)
        {
            return _context.Protocols.Where(p => p.Id == protocolId).Select(o => o.Organization).FirstOrDefault();
        }

        public ICollection<UserOrganizationRole> GetUserOrganizationRoleEntriesByOrganization(long id)
        {
            return _context.UserOrganizationRoles.Where(au => au.Organization.Id == id).ToList();
        }

        public bool CreateOrganization(Organization organization)
        {
            _context.Add(organization);
            return Save();
        }

        public bool UpdateOrganization(Organization organization)
        {
            _context.Update(organization);
            return Save();
        }

        public bool DeleteOrganization(Organization organization)
        {
            _context.Remove(organization);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public Organization GetOwningOrganizationByTemplates(long templateId)
        {
            return _context.Templates.Where(p => p.Id == templateId).Select(o => o.Organization).FirstOrDefault();
        }
    }
}