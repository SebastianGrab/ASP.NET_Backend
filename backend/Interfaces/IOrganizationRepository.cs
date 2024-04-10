using Models;

namespace Interfaces
{
    public interface IOrganizationRepository
    {
        ICollection<Organization> GetOrganizations();
        Organization GetOrganization(long id);
        bool OrganizationExists(long id);
        ICollection<Organization> GetOrganizationsByTemplate(long templateId);
        Organization GetOwningOrganizationByTemplates(long templateId);
        ICollection<Organization> GetOrganizationsByUser(long userId);
        public ICollection<UserOrganizationRole> GetUserOrganizationRoleEntriesByOrganization(long id);
        Organization GetOrganizationByProtocol(long protocolId);
        bool CreateOrganization(Organization organization);
        bool UpdateOrganization(Organization organization);
        bool DeleteOrganization(Organization organization);
        bool Save();
    }
}

