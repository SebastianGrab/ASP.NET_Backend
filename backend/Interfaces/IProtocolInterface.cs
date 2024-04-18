using Helper;
using Helper.SearchObjects;
using Models;

namespace Interfaces
{
    public interface IProtocolRepository
    {
        ICollection<Protocol> GetProtocols(QueryObject dateQuery, ProtocolSearchObject protocolSearch);
        Protocol GetProtocol(long id);
        bool ProtocolExists(long id);
        ICollection<Protocol> GetProtocolsByUser(long userId, QueryObject dateQuery, ProtocolSearchObject protocolSearch);
        ICollection<Protocol> GetProtocolsByAdditionalUser(long additionalUserId, QueryObject dateQuery, ProtocolSearchObject protocolSearch);
        ICollection<Protocol> GetProtocolsByTemplate(long templateId, QueryObject dateQuery, ProtocolSearchObject protocolSearch);
        ICollection<Protocol> GetProtocolsByOrganization(long organizationId, QueryObject dateQuery, ProtocolSearchObject protocolSearch);
        bool CreateProtocol(List<long> additionalUserIds, Protocol protocol);
        bool UpdateProtocol(List<long> additionalUserIds, Protocol protocol);
        bool DeleteProtocol(Protocol protocol);
        bool Save();
    }
}

