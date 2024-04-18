using Data;
using Helper;
using Helper.SeachObjects;
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

        public ICollection<Organization> GetOrganizations(QueryObject? dateQuery, OrganizationSearchObject? organizationSearch)
        {
            var orgas = _context.Organizations.OrderByDescending(o => o.Id).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(organizationSearch.Name))
            {
                orgas = orgas.Where(o => o.Name.Contains(organizationSearch.Name));
            }

            if(!string.IsNullOrWhiteSpace(organizationSearch.OrganizationType))
            {
                orgas = orgas.Where(o => o.OrganizationType.Contains(organizationSearch.OrganizationType));
            }

            return orgas.OrderByDescending(p => p.Id).ToList();
        }

        public Organization GetOrganization(long id)
        {
            return _context.Organizations.Where(o => o.Id == id).FirstOrDefault();
        }

        public bool OrganizationExists(long id)
        {
            return _context.Organizations.Any(o => o.Id == id);
        }

        public ICollection<Organization> GetOrganizationsByUser(long userId, QueryObject dateQuery, OrganizationSearchObject organizationSearch)
        {
            var orgas = _context.UserOrganizationRoles.Where(uor => uor.User.Id == userId).Select(o => o.Organization).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(organizationSearch.Name))
            {
                orgas = orgas.Where(o => o.Name.Contains(organizationSearch.Name));
            }

            if(!string.IsNullOrWhiteSpace(organizationSearch.OrganizationType))
            {
                orgas = orgas.Where(o => o.OrganizationType.Contains(organizationSearch.OrganizationType));
            }

            return orgas.OrderByDescending(p => p.Id).ToList();
        }

        public ICollection<Organization> GetOrganizationsByTemplate(long templateId, QueryObject dateQuery, OrganizationSearchObject organizationSearch)
        {
            var orgas = _context.TemplateOrganizations.Where(to => to.Template.Id == templateId).Select(o => o.Organization).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(organizationSearch.Name))
            {
                orgas = orgas.Where(o => o.Name.Contains(organizationSearch.Name));
            }

            if(!string.IsNullOrWhiteSpace(organizationSearch.OrganizationType))
            {
                orgas = orgas.Where(o => o.OrganizationType.Contains(organizationSearch.OrganizationType));
            }

            return orgas.OrderByDescending(p => p.Id).ToList();
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

        public ICollection<Organization> GetOrganizationDaughters(long organizationId, QueryObject dateQuery, OrganizationSearchObject organizationSearch)
        {
            var orgas = _context.Organizations.Where(o => o.parentId == organizationId).OrderByDescending(o => o.Id).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                orgas = orgas.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(organizationSearch.Name))
            {
                orgas = orgas.Where(o => o.Name.Contains(organizationSearch.Name));
            }

            if(!string.IsNullOrWhiteSpace(organizationSearch.OrganizationType))
            {
                orgas = orgas.Where(o => o.OrganizationType.Contains(organizationSearch.OrganizationType));
            }

            return orgas.OrderByDescending(p => p.Id).ToList();
        }
    }
}