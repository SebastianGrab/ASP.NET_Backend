using System.Security.Claims;
using csharp.Dto.Statistics;
using Helper;
using Models;

namespace Interfaces
{
    public interface IStatisticsRepository
    {
        int GetNumberOfProtocols(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics);
        int NoOfUsersInCurrentUsersOrganizations(ClaimsPrincipal claimsPrincipal);
        IEnumerable<ProtocolYearCount> GetProtocolsAggregatedByYear(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics);
        IEnumerable<ProtocolMonthlyCount> GetProtocolsAggregatedByMonth(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics);
        IEnumerable<ProtocolDateCount> GetProtocolsAggregatedByDate(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics);
        IEnumerable<ProtocolWeekdayCount> GetProtocolsAggregatedByWeekDay(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics);
        IEnumerable<ProtocolTimeCount> GetProtocolsAggregatedByTime(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics);
        IEnumerable<ProtocolUserCount> GetProtocolsAggregatedByUser(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics);
        IEnumerable<ProtocolOrganizationCount> GetProtocolsAggregatedByOrganization(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics);
        IEnumerable<ProtocolTypeCount> GetProtocolsAggregatedByType(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics);
        IEnumerable<ProtocolPlaceCount> GetProtocolsAggregatedByPlace(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics);
        IEnumerable<ProtocolNacaCount> GetProtocolsAggregatedByNACA(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics);
        IEnumerable<ProtocolImpactCount> GetProtocolsAggregatedByImpact(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics);
    }
}

