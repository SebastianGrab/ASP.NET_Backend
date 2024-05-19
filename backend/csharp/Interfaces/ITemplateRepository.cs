using Helper;
using Helper.SearchObjects;
using Models;

namespace Interfaces
{
    public interface ITemplateRepository
    {
        ICollection<Template> GetTemplates(QueryObject dateQuery, TemplateSearchObject templateSearch);
        Template GetTemplate(long id);
        bool TemplateExists(long id);
        ICollection<Template> GetSharedTemplatesByOrganization(long organizationId, QueryObject dateQuery, TemplateSearchObject templateSearch);
        ICollection<Template> GetTemplatesOwnedByOrganization(long organizationId, QueryObject dateQuery, TemplateSearchObject templateSearch);
        Template GetTemplateByProtocol(long protocolId);
        bool CreateTemplate(long organizationId, Template template);
        bool UpdateTemplate(Template template);
        bool DeleteTemplate(Template template);
        bool Save();
    }
}

