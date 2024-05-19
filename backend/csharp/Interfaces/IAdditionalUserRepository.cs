using Models;

namespace Interfaces
{
    public interface IAdditionalUserRepository
    {
        AdditionalUser GetAdditionalUser(long userId, long protocolId);
        ICollection<AdditionalUser> GetAdditionalUserEntriesByUser(long id);
        ICollection<AdditionalUser> GetAdditionalUserEntriesByProtocol(long id);
        bool DeleteAdditionalUserEntries(List<AdditionalUser> additionalUsers);
        bool DeleteAdditionalUserEntry(AdditionalUser additionalUser);
        bool AdditionalUserExists(long userId, long protocolId);
        bool Save();
    }
}

