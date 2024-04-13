using Data;
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

        public ICollection<Template> GetTemplates()
        {
            return _context.Templates.OrderByDescending(t => t.Id).ToList();
        }

        public ICollection<Template> GetSharedTemplatesByOrganization(long organizationId)
        {
            return _context.TemplateOrganizations.Where(to => to.Organization.Id == organizationId).Select(t => t.Template).OrderByDescending(t => t.Id).ToList();
        }

        public ICollection<Template> GetTemplatesOwnedByOrganization(long organizationId)
        {
            return _context.Templates.Where(p => p.Organization.Id == organizationId).OrderByDescending(t => t.Id).ToList();
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