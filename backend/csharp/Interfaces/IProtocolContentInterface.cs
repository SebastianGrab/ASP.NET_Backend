using System.Security.Claims;
using Models;

namespace Interfaces
{
    public interface IProtocolContentRepository
    {
        ProtocolContent GetProtocolContent(long protocolId, ClaimsPrincipal claimUser);
        bool ProtocolContentExists(long protocolId);
        bool CreateProtocolContent(ProtocolContent protocolContent);
        bool UpdateProtocolContent(ProtocolContent protocolContent);
        bool DeleteProtocolContent(ProtocolContent protocolContent);
        bool Save();
    }
}

