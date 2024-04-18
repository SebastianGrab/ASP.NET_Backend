using Models;
using Helper;
using Helper.SeachObjects;

namespace Interfaces
{
    public interface IOrganizationRepository
    {
        ICollection<Organization> GetOrganizations(QueryObject dateQuery, OrganizationSearchObject organizationSearch);
        Organization GetOrganization(long id);
        bool OrganizationExists(long id);
        ICollection<Organization> GetOrganizationsByTemplate(long templateId, QueryObject dateQuery, OrganizationSearchObject organizationSearch);
        Organization GetOwningOrganizationByTemplates(long templateId);
        ICollection<Organization> GetOrganizationsByUser(long userId, QueryObject dateQuery, OrganizationSearchObject organizationSearch);
        ICollection<Organization> GetOrganizationDaughters(long organizationId, QueryObject dateQuery, OrganizationSearchObject organizationSearch);
        public ICollection<UserOrganizationRole> GetUserOrganizationRoleEntriesByOrganization(long id);
        Organization GetOrganizationByProtocol(long protocolId);
        bool CreateOrganization(Organization organization);
        bool UpdateOrganization(Organization organization);
        bool DeleteOrganization(Organization organization);
        bool Save();
    }
}

