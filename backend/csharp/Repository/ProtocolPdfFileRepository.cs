using Data;
using Interfaces;
using Models;

namespace Repository
{
    public class ProtocolPdfFileRepository : IProtocolPdfFileRepository
    {
        private readonly ProtocolContext _context;
        
        public ProtocolPdfFileRepository(ProtocolContext context)
        {
            _context = context;
        }

        public bool CreateProtocolPdfFile(ProtocolPdfFile protocolPdfFile)
        {
            _context.Add(protocolPdfFile);
            return Save();
        }

        public bool DeleteProtocolPdfFile(ProtocolPdfFile protocolPdfFile)
        {
            _context.Remove(protocolPdfFile);
            return Save();
        }

        public ProtocolPdfFile GetProtocolPdfFile(long protocolId)
        {
            return _context.ProtocolPdfFiles.Where(ppf => ppf.protocolId == protocolId).FirstOrDefault();
        }

        public bool ProtocolPdfFileExists(long protocolId)
        {
            return _context.ProtocolPdfFiles.Any(ppf => ppf.protocolId == protocolId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateProtocolPdfFile(ProtocolPdfFile protocolPdfFile)
        {
            _context.Update(protocolPdfFile);
            return Save();
        }
    }
}