using Models;

namespace Interfaces
{
    public interface IProtocolPdfFileRepository
    {
        ProtocolPdfFile GetProtocolPdfFile(long protocolId);
        bool ProtocolPdfFileExists(long protocolId);
        bool CreateProtocolPdfFile(ProtocolPdfFile protocolPdfFile);
        bool UpdateProtocolPdfFile(ProtocolPdfFile protocolPdfFile);
        bool DeleteProtocolPdfFile(ProtocolPdfFile protocolPdfFile);
        bool Save();
    }
}

