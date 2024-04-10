using Models;

namespace Interfaces
{
    public interface ITemplateRepository
    {
        ICollection<Template> GetTemplates();
        Template GetTemplate(long id);
        bool TemplateExists(long id);
        ICollection<Template> GetSharedTemplatesByOrganization(long organizationId);
        ICollection<Template> GetTemplatesOwnedByOrganization(long organizationId);
        Template GetTemplateByProtocol(long protocolId);
        bool CreateTemplate(long organizationId, Template template);
        bool UpdateTemplate(Template template);
        bool DeleteTemplate(Template template);
        bool Save();
    }
}

