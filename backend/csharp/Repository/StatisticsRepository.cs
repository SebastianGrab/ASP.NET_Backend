using System.Security.Claims;
using Data;
using Interfaces;
using Models;

namespace Repository
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ProtocolContext _context;
        
        public StatisticsRepository(ProtocolContext context)
        {
            _context = context;
        }

        public int GetNumberOfOrganizations()
        {
            return _context.Organizations.OrderBy(o => o.Id).Count();
        }

        public int GetNumberOfProtocolsPerUser(long userId)
        {
            return _context.Protocols.Where(p => p.User.Id == userId).Count();
        }

        public int GetNumberOfProtocols(ClaimsPrincipal claimsPrincipal)
        {
            return 0;
        }
    }
}