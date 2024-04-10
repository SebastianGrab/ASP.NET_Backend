using Models;

namespace Interfaces
{
    public interface IProtocolRepository
    {
        ICollection<Protocol> GetProtocols();
        Protocol GetProtocol(long id);
        bool ProtocolExists(long id);
        ICollection<Protocol> GetProtocolsByUser(long userId);
        ICollection<Protocol> GetProtocolsByAdditionalUser(long additionalUserId);
        ICollection<Protocol> GetProtocolsByTemplate(long templateId);
        ICollection<Protocol> GetProtocolsByOrganization(long organizationId);
        bool CreateProtocol(List<long> additionalUserIds, Protocol protocol);
        bool UpdateProtocol(List<long> additionalUserIds, Protocol protocol);
        bool DeleteProtocol(Protocol protocol);
        bool Save();
    }
}

