using Data;
using Helper;
using Helper.SearchObjects;
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

        public ICollection<Protocol> GetProtocols(QueryObject dateQuery, ProtocolSearchObject protocolSearch)
        {
            var protocols = _context.Protocols.OrderByDescending(p => p.Id).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.Name))
            {
                protocols = protocols.Where(o => o.Name.Contains(protocolSearch.Name));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ReviewComment))
            {
                protocols = protocols.Where(o => o.ReviewComment.Contains(protocolSearch.ReviewComment));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ProtocolContent))
            {
                protocols = protocols.Where(o => o.ProtocolContent.Content.Contains(protocolSearch.ProtocolContent));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OwnerName))
            {
                protocols = protocols.Where(o => o.User.Username.Contains(protocolSearch.OwnerName));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OrganizationName))
            {
                protocols = protocols.Where(o => o.Organization.Name.Contains(protocolSearch.OrganizationName));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.UserName))
            {
                protocols = protocols.Where(o => o.AdditionalUser.Select(u => u.User.Username).Contains(protocolSearch.UserName));
            }

            return protocols.OrderByDescending(p => p.Id).ToList();
        }

        public ICollection<Protocol> GetProtocolsByAdditionalUser(long additionalUserId, QueryObject dateQuery, ProtocolSearchObject protocolSearch)
        {
            var protocols = _context.AdditionalUsers.Where(au => au.userId == additionalUserId).Select(p => p.Protocol).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.Name))
            {
                protocols = protocols.Where(o => o.Name.Contains(protocolSearch.Name));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ReviewComment))
            {
                protocols = protocols.Where(o => o.ReviewComment.Contains(protocolSearch.ReviewComment));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ProtocolContent))
            {
                protocols = protocols.Where(o => o.ProtocolContent.Content.Contains(protocolSearch.ProtocolContent));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OwnerName))
            {
                protocols = protocols.Where(o => o.User.Username.Contains(protocolSearch.OwnerName));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OrganizationName))
            {
                protocols = protocols.Where(o => o.Organization.Name.Contains(protocolSearch.OrganizationName));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.UserName))
            {
                protocols = protocols.Where(o => o.AdditionalUser.Select(u => u.User.Username).Contains(protocolSearch.UserName));
            }

            return protocols.OrderByDescending(p => p.Id).ToList();
        }

        public ICollection<Protocol> GetProtocolsByOrganization(long organizationId, QueryObject dateQuery, ProtocolSearchObject protocolSearch)
        {
            var protocols = _context.Protocols.Where(p => p.Organization.Id == organizationId).OrderByDescending(p => p.Id).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.Name))
            {
                protocols = protocols.Where(o => o.Name.Contains(protocolSearch.Name));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ReviewComment))
            {
                protocols = protocols.Where(o => o.ReviewComment.Contains(protocolSearch.ReviewComment));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ProtocolContent))
            {
                protocols = protocols.Where(o => o.ProtocolContent.Content.Contains(protocolSearch.ProtocolContent));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OwnerName))
            {
                protocols = protocols.Where(o => o.User.Username.Contains(protocolSearch.OwnerName));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OrganizationName))
            {
                protocols = protocols.Where(o => o.Organization.Name.Contains(protocolSearch.OrganizationName));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.UserName))
            {
                protocols = protocols.Where(o => o.AdditionalUser.Select(u => u.User.Username).Contains(protocolSearch.UserName));
            }

            return protocols.OrderByDescending(p => p.Id).ToList();
        }

        public ICollection<Protocol> GetProtocolsByTemplate(long templateId, QueryObject dateQuery, ProtocolSearchObject protocolSearch)
        {
            var protocols = _context.Protocols.OrderBy(p => p.Template.Id == templateId).OrderByDescending(p => p.Id).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.Name))
            {
                protocols = protocols.Where(o => o.Name.Contains(protocolSearch.Name));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ReviewComment))
            {
                protocols = protocols.Where(o => o.ReviewComment.Contains(protocolSearch.ReviewComment));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ProtocolContent))
            {
                protocols = protocols.Where(o => o.ProtocolContent.Content.Contains(protocolSearch.ProtocolContent));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OwnerName))
            {
                protocols = protocols.Where(o => o.User.Username.Contains(protocolSearch.OwnerName));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OrganizationName))
            {
                protocols = protocols.Where(o => o.Organization.Name.Contains(protocolSearch.OrganizationName));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.UserName))
            {
                protocols = protocols.Where(o => o.AdditionalUser.Select(u => u.User.Username).Contains(protocolSearch.UserName));
            }

            return protocols.OrderByDescending(p => p.Id).ToList();
        }

        public ICollection<Protocol> GetProtocolsByUser(long userId, QueryObject dateQuery, ProtocolSearchObject protocolSearch)
        {
            var protocols = _context.Protocols.Where(p => p.User.Id == userId).OrderByDescending(p => p.Id).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                protocols = protocols.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.Name))
            {
                protocols = protocols.Where(o => o.Name.Contains(protocolSearch.Name));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ReviewComment))
            {
                protocols = protocols.Where(o => o.ReviewComment.Contains(protocolSearch.ReviewComment));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ProtocolContent))
            {
                protocols = protocols.Where(o => o.ProtocolContent.Content.Contains(protocolSearch.ProtocolContent));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OwnerName))
            {
                protocols = protocols.Where(o => o.User.Username.Contains(protocolSearch.OwnerName));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OrganizationName))
            {
                protocols = protocols.Where(o => o.Organization.Name.Contains(protocolSearch.OrganizationName));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.UserName))
            {
                protocols = protocols.Where(o => o.AdditionalUser.Select(u => u.User.Username).Contains(protocolSearch.UserName));
            }

            return protocols.OrderByDescending(p => p.Id).ToList();
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