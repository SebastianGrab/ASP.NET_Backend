using System.Security.Claims;
using Models;

namespace Interfaces
{
    public interface IStatisticsRepository
    {
        int GetNumberOfProtocolsPerUser(long userId);
        int GetNumberOfOrganizations();
        int GetNumberOfProtocols(ClaimsPrincipal claimsPrincipal);
    }
}

