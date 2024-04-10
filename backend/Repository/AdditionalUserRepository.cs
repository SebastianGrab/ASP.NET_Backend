using Data;
using Interfaces;
using Models;

namespace Repository
{
    public class AdditionalUserRepository : IAdditionalUserRepository
    {
        private readonly ProtocolContext _context;
        
        public AdditionalUserRepository(ProtocolContext context)
        {
            _context = context;
        }

        public ICollection<AdditionalUser> GetAdditionalUserEntriesByUser(long id)
        {
            return _context.AdditionalUsers.Where(au => au.User.Id == id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool DeleteAdditionalUserEntries(List<AdditionalUser> additionalUsers)
        {
            _context.RemoveRange(additionalUsers);
            return Save();
        }

        public ICollection<AdditionalUser> GetAdditionalUserEntriesByProtocol(long id)
        {
            return _context.AdditionalUsers.Where(au => au.Protocol.Id == id).ToList();
        }

        public bool AdditionalUserExists(long userId, long protocolId)
        {
            return _context.AdditionalUsers.Any(to => to.User.Id == userId && to.Protocol.Id == protocolId);
        }

        public bool DeleteAdditionalUserEntry(AdditionalUser additionalUser)
        {
            _context.Remove(additionalUser);
            return Save();
        }

        public AdditionalUser GetAdditionalUser(long userId, long protocolId)
        {
            return _context.AdditionalUsers.Where(au => au.User.Id == userId && au.Protocol.Id == protocolId).FirstOrDefault();
        }
    }
}