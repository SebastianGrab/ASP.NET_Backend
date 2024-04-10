using Data;
using Interfaces;
using Models;

namespace Repository
{
    public class TemplateOrganizationRepository : ITemplateOrganizationRepository
    {
        private readonly ProtocolContext _context;
        
        public TemplateOrganizationRepository(ProtocolContext context)
        {
            _context = context;
        }

        public bool AddTemplateToOrganization(long organizationId, long templateId)
        {
            var organizationEntity = _context.Organizations.Where(o => o.Id == organizationId).FirstOrDefault();
            var templateEntity = _context.Templates.Where(o => o.Id == templateId).FirstOrDefault();

            var templateOrganization = new TemplateOrganization()
            {
                organizationId = organizationId,
                Organization = organizationEntity,
                templateId = templateId,
                Template = templateEntity
            };

            _context.Add(templateOrganization);

            return Save();
        }

        public bool DeleteTemplateOrganizationEntries(List<TemplateOrganization> templateOrganizations)
        {
            _context.RemoveRange(templateOrganizations);
            return Save();
        }

        public ICollection<TemplateOrganization> GetTemplateOrganizationEntriesByTemplate(long id)
        {
            return _context.TemplateOrganizations.Where(au => au.Template.Id == id).ToList();
        }

        public ICollection<TemplateOrganization> GetTemplateOrganizationEntriesByOrganization(long id)
        {
            return _context.TemplateOrganizations.Where(au => au.Organization.Id == id).ToList();
        }

        public bool RemoveTemplateFromOrganization(long organizationId, long templateId)
        {
            var templateOrganization = _context.TemplateOrganizations.Where(to => to.organizationId == organizationId && to.templateId == templateId).FirstOrDefault();
            _context.Remove(templateOrganization);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool TemplateOrganizationExists(long templateId, long organizationId)
        {
            return _context.TemplateOrganizations.Any(to => to.templateId == templateId && to.organizationId == organizationId);
        }
    }
}