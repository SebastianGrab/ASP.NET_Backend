using csharp.Models;

namespace csharp.Interfaces
{
    public interface ITemplateVersionRepository
    {
        ICollection<TemplateVersions> GetVersions(long templateId);
        bool CreateTemplateVersion(TemplateVersions templateVersion);
        bool Save();
        
    }
}