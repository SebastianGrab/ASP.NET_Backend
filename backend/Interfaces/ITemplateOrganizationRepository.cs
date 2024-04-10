using Models;

namespace Interfaces
{
    public interface ITemplateOrganizationRepository
    {
        ICollection<TemplateOrganization> GetTemplateOrganizationEntriesByTemplate(long id);
        ICollection<TemplateOrganization> GetTemplateOrganizationEntriesByOrganization(long id);
        bool AddTemplateToOrganization(long organizationId, long templateId);
        bool RemoveTemplateFromOrganization(long organizationId, long templateId);
        bool DeleteTemplateOrganizationEntries(List<TemplateOrganization> templateOrganizations);
        bool TemplateOrganizationExists(long templateId, long organizationId);
        bool Save();
    }
}

