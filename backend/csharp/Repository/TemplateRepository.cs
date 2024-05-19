using Data;
using Helper;
using Helper.SearchObjects;
using Interfaces;
using Models;

namespace Repository
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly ProtocolContext _context;
        
        public TemplateRepository(ProtocolContext context)
        {
            _context = context;
        }

        public bool CreateTemplate(long organizationId, Template template)
        {
            var templateOrganizationEntity = _context.Organizations.Where(o => o.Id == organizationId).FirstOrDefault();

            var templateOrganization = new TemplateOrganization()
            {
                organizationId = organizationId,
                Organization = templateOrganizationEntity,
                templateId = template.Id,
                Template = template
            };

            _context.Add(templateOrganization);

            _context.Add(template);

            return Save();
        }

        public bool DeleteTemplate(Template template)
        {
            _context.Remove(template);
            return Save();
        }

        public Template GetTemplate(long id)
        {
            return _context.Templates.Where(t => t.Id == id).FirstOrDefault();
        }

        public Template GetTemplateByProtocol(long protocolId)
        {
            return _context.Protocols.Where(p => p.Id == protocolId).Select(t => t.Template).FirstOrDefault();
        }

        public ICollection<Template> GetTemplates(QueryObject dateQuery, TemplateSearchObject templateSearch)
        {
            var templates = _context.Templates.OrderByDescending(t => t.Id).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                templates = templates.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                templates = templates.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                templates = templates.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                templates = templates.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(templateSearch.Name))
            {
                templates = templates.Where(o => o.Name.Contains(templateSearch.Name));
            }

            if(!string.IsNullOrWhiteSpace(templateSearch.Description))
            {
                templates = templates.Where(o => o.Description.Contains(templateSearch.Description));
            }

            return templates.OrderByDescending(p => p.Id).ToList();
        }

        public ICollection<Template> GetSharedTemplatesByOrganization(long organizationId, QueryObject dateQuery, TemplateSearchObject templateSearch)
        {
            var templates = _context.TemplateOrganizations.Where(to => to.Organization.Id == organizationId).Select(t => t.Template).OrderByDescending(t => t.Id).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                templates = templates.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                templates = templates.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                templates = templates.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                templates = templates.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(templateSearch.Name))
            {
                templates = templates.Where(o => o.Name.Contains(templateSearch.Name));
            }

            if(!string.IsNullOrWhiteSpace(templateSearch.Description))
            {
                templates = templates.Where(o => o.Description.Contains(templateSearch.Description));
            }

            return templates.OrderByDescending(p => p.Id).ToList();
        }

        public ICollection<Template> GetTemplatesOwnedByOrganization(long organizationId, QueryObject dateQuery, TemplateSearchObject templateSearch)
        {
            var templates = _context.Templates.Where(p => p.Organization.Id == organizationId).OrderByDescending(t => t.Id).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                templates = templates.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                templates = templates.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                templates = templates.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                templates = templates.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(templateSearch.Name))
            {
                templates = templates.Where(o => o.Name.Contains(templateSearch.Name));
            }

            if(!string.IsNullOrWhiteSpace(templateSearch.Description))
            {
                templates = templates.Where(o => o.Description.Contains(templateSearch.Description));
            }

            return templates.OrderByDescending(p => p.Id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool TemplateExists(long id)
        {
            return _context.Templates.Any(t => t.Id == id);
        }

        public bool UpdateTemplate(Template template)
        {
            _context.Update(template);
            return Save();
        }
    }
}