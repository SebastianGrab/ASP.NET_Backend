using System.Security.Claims;
using Helper;
using Helper.SearchObjects;
using Models;

namespace Interfaces
{
    public interface IProtocolRepository
    {
        ICollection<Protocol> GetProtocols(QueryObject dateQuery, ProtocolSearchObject protocolSearch, ClaimsPrincipal claimUser);
        Protocol GetProtocol(long id, ClaimsPrincipal claimUser);
        bool ProtocolExists(long id);
        ICollection<Protocol> GetProtocolsByUser(long userId, QueryObject dateQuery, ProtocolSearchObject protocolSearch, ClaimsPrincipal claimUser);
        ICollection<Protocol> GetProtocolsByAdditionalUser(long additionalUserId, QueryObject dateQuery, ProtocolSearchObject protocolSearch, ClaimsPrincipal claimUser);
        ICollection<Protocol> GetProtocolsByTemplate(long templateId, QueryObject dateQuery, ProtocolSearchObject protocolSearch, ClaimsPrincipal claimUser);
        ICollection<Protocol> GetProtocolsByOrganization(long organizationId, QueryObject dateQuery, ProtocolSearchObject protocolSearch, ClaimsPrincipal claimUser);
        bool CreateProtocol(List<long> additionalUserIds, Protocol protocol);
        bool UpdateProtocol(long closingUserId, List<long> additionalUserIds, Protocol protocol);
        bool DeleteProtocol(Protocol protocol);
        int GetNumberOfProtocolsToReview(ClaimsPrincipal claimUser);
        bool Save();
    }
}

