using Data;
using Interfaces;
using Models;

namespace Repository
{
    public class ProtocolContentRepository : IProtocolContentRepository
    {
        private readonly ProtocolContext _context;
        
        public ProtocolContentRepository(ProtocolContext context)
        {
            _context = context;
        }

        public bool CreateProtocolContent(ProtocolContent protocolContent)
        {
            _context.Add(protocolContent);
            return Save();
        }

        public bool DeleteProtocolContent(ProtocolContent protocolContent)
        {
            _context.Remove(protocolContent);
            return Save();
        }

        public ProtocolContent GetProtocolContent(long protocolId)
        {
            return _context.ProtocolContents.Where(pc => pc.protocolId == protocolId).FirstOrDefault();
        }

        public bool ProtocolContentExists(long protocolId)
        {
            return _context.ProtocolContents.Any(pc => pc.protocolId == protocolId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateProtocolContent(ProtocolContent protocolContent)
        {
            _context.Update(protocolContent);
            return Save();
        }
    }
}