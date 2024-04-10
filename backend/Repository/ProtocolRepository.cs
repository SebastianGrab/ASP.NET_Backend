using Data;
using Interfaces;
using Models;

namespace Repository
{
    public class ProtocolRepository : IProtocolRepository
    {
        private readonly ProtocolContext _context;
        
        public ProtocolRepository(ProtocolContext context)
        {
            _context = context;
        }

        public bool CreateProtocol(List<long> additionalUserIds, Protocol protocol)
        {
            _context.Add(protocol);
            
            foreach(var additionalUserId in additionalUserIds)
            {
                var additionalUserEntity = _context.Users.Where(a => a.Id == additionalUserId).FirstOrDefault();

                var additionalUser = new AdditionalUser()
                {
                    userId = additionalUserEntity.Id,
                    User = additionalUserEntity,
                    protocolId = protocol.Id,
                    Protocol = protocol,
                };

                _context.Add(additionalUser);
            }

            return Save();
        }

        public bool DeleteProtocol(Protocol protocol)
        {
            _context.Remove(protocol);
            return Save();
        }

        public Protocol GetProtocol(long id)
        {
            return _context.Protocols.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Protocol> GetProtocols()
        {
            return _context.Protocols.OrderBy(p => p.Id).ToList();
        }

        public ICollection<Protocol> GetProtocolsByAdditionalUser(long additionalUserId)
        {
            return _context.AdditionalUsers.Where(au => au.userId == additionalUserId).Select(p => p.Protocol).OrderBy(p => p.Id).ToList();
        }

        public ICollection<Protocol> GetProtocolsByOrganization(long organizationId)
        {
            return _context.Protocols.Where(p => p.Organization.Id == organizationId).OrderBy(p => p.Id).ToList();
        }

        public ICollection<Protocol> GetProtocolsByTemplate(long templateId)
        {
            return _context.Protocols.OrderBy(p => p.Template.Id == templateId).OrderBy(p => p.Id).ToList();
        }

        public ICollection<Protocol> GetProtocolsByUser(long userId)
        {
            return _context.Protocols.Where(p => p.User.Id == userId).OrderBy(p => p.Id).ToList();
        }

        public bool ProtocolExists(long id)
        {
            return _context.Protocols.Any(p => p.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateProtocol(List<long> additionalUserIds, Protocol protocol)
        {
            foreach(var additionalUserId in additionalUserIds)
            {
                var additionalUserEntity = _context.Users.Where(a => a.Id == additionalUserId).FirstOrDefault();

                var additionalUser = new AdditionalUser()
                {
                    userId = additionalUserEntity.Id,
                    User = additionalUserEntity,
                    protocolId = protocol.Id,
                    Protocol = protocol,
                };

                _context.Add(additionalUser);
            }
            
            _context.Update(protocol);
            return Save();
        }
    }
}