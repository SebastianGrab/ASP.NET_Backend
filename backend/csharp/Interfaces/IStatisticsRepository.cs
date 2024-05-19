using Models;

namespace Interfaces
{
    public interface IStatisticsRepository
    {
        int GetNumberOfProtocols();
        int GetNumberOfProtocolsPerUser(long userId);
        int GetNumberOfOrganizations();
    }
}

