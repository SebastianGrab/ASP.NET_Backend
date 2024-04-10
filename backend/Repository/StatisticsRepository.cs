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

        public int GetNumberOfProtocols()
        {
            return _context.Protocols.OrderBy(p => p.Id).Count();
        }

        public int GetNumberOfProtocolsPerUser(long userId)
        {
            return _context.Protocols.OrderBy(p => p.User.Id == userId).Count();
        }
    }
}