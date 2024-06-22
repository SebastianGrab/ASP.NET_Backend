using csharp.Interfaces;
using csharp.Models;
using Data;

namespace Repository
{
    public class TemplateVersionRepository : ITemplateVersionRepository
    {
        private readonly ProtocolContext _context;
        
        public TemplateVersionRepository(ProtocolContext context)
        {
            _context = context;
        }

        public bool CreateTemplateVersion(TemplateVersions templateVersion)
        {
            _context.Add(templateVersion);
            return Save();
        }

        public ICollection<TemplateVersions> GetVersions(long templateId)
        {
            var templateVersions = _context.TemplateVersions.Where(t => t.templateId == templateId).OrderByDescending(t => t.Id).ToList();

            return templateVersions;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}