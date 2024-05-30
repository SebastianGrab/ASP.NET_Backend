using System.Security.Claims;
using Models;

namespace Interfaces
{
    public interface IProtocolPdfFileRepository
    {
        ProtocolPdfFile GetProtocolPdfFile(long protocolId, ClaimsPrincipal claimUser);
        bool ProtocolPdfFileExists(long protocolId);
        bool CreateProtocolPdfFile(ProtocolPdfFile protocolPdfFile);
        bool UpdateProtocolPdfFile(ProtocolPdfFile protocolPdfFile);
        bool DeleteProtocolPdfFile(ProtocolPdfFile protocolPdfFile);
        bool Save();
    }
}

