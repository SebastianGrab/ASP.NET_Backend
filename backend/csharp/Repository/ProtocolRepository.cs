using System.Security.Claims;
using Data;
using Helper;
using Helper.SearchObjects;
using Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public int GetNumberOfProtocolsToReview(ClaimsPrincipal claimUser)
        {
            var protocols = _context.Protocols.Where(p => p.IsDraft == false && p.IsClosed == false).AsQueryable();

            var claimRoles = claimUser.GetRoles();
            var claimOrganizationIds = claimUser.GetOrganizationIds();
            var claimUserId = claimUser.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId == claimUserId).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId || additionalUserProtocolIds.Contains(p.Id));
            }
            else
            {
                protocols = null;
            }

            return protocols.Count();
        }

        public Protocol GetProtocol(long id, ClaimsPrincipal claimUser)
        {
            var protocols = _context.Protocols.Where(p => p.Id == id).AsQueryable();

            var claimRoles = claimUser.GetRoles();
            var claimOrganizationIds = claimUser.GetOrganizationIds();
            var claimUserId = claimUser.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId == claimUserId).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId || additionalUserProtocolIds.Contains(p.Id));
                protocols = protocols.Where(p => p.UpdatedDate >= DateTime.UtcNow.AddDays(-42));
            }
            else
            {
                protocols = null;
            }

            return protocols.FirstOrDefault();
        }

        public ICollection<Protocol> GetProtocols(QueryObject dateQuery, ProtocolSearchObject protocolSearch, ClaimsPrincipal claimUser)
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
                protocols = protocols.Where(o => o.Name.ToLower().Contains(protocolSearch.Name.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ReviewComment))
            {
                protocols = protocols.Where(o => o.ReviewComment.ToLower().Contains(protocolSearch.ReviewComment.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ProtocolContent))
            {
                protocols = protocols.Where(o => o.ProtocolContent.Content.ToLower().Contains(protocolSearch.ProtocolContent.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OwnerName))
            {
                protocols = protocols.Where(o => (o.User.FirstName + " " + o.User.LastName).ToLower().Contains(protocolSearch.OwnerName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OrganizationName))
            {
                protocols = protocols.Where(o => o.Organization.Name.ToLower().Contains(protocolSearch.OrganizationName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.UserName))
            {
                protocols = protocols.Where(o => o.AdditionalUser.Select(u => u.User.FirstName.ToLower() + " " + u.User.LastName.ToLower()).Contains(protocolSearch.UserName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.IsClosed.ToString()))
            {
                protocols = protocols.Where(o => o.IsClosed == protocolSearch.IsClosed);
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.IsDraft.ToString()))
            {
                protocols = protocols.Where(o => o.IsDraft == protocolSearch.IsDraft);
            }

            var claimRoles = claimUser.GetRoles();
            var claimOrganizationIds = claimUser.GetOrganizationIds();
            var claimUserId = claimUser.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId == claimUserId).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId || additionalUserProtocolIds.Contains(p.Id));
                protocols = protocols.Where(p => p.UpdatedDate >= DateTime.UtcNow.AddDays(-42));
            }
            else
            {
                protocols = null;
            }

            return protocols.OrderByDescending(p => p.Id).ToList();
        }

        public ICollection<Protocol> GetProtocolsByAdditionalUser(long additionalUserId, QueryObject dateQuery, ProtocolSearchObject protocolSearch, ClaimsPrincipal claimUser)
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
                protocols = protocols.Where(o => o.Name.ToLower().Contains(protocolSearch.Name.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ReviewComment))
            {
                protocols = protocols.Where(o => o.ReviewComment.ToLower().Contains(protocolSearch.ReviewComment.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ProtocolContent))
            {
                protocols = protocols.Where(o => o.ProtocolContent.Content.ToLower().Contains(protocolSearch.ProtocolContent.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OwnerName))
            {
                protocols = protocols.Where(o => (o.User.FirstName + " " + o.User.LastName).ToLower().Contains(protocolSearch.OwnerName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OrganizationName))
            {
                protocols = protocols.Where(o => o.Organization.Name.ToLower().Contains(protocolSearch.OrganizationName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.UserName))
            {
                protocols = protocols.Where(o => o.AdditionalUser.Select(u => u.User.FirstName.ToLower() + " " + u.User.LastName.ToLower()).Contains(protocolSearch.UserName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.IsClosed.ToString()))
            {
                protocols = protocols.Where(o => o.IsClosed == protocolSearch.IsClosed);
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.IsDraft.ToString()))
            {
                protocols = protocols.Where(o => o.IsDraft == protocolSearch.IsDraft);
            }

            var claimRoles = claimUser.GetRoles();
            var claimOrganizationIds = claimUser.GetOrganizationIds();
            var claimUserId = claimUser.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId == claimUserId).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId || additionalUserProtocolIds.Contains(p.Id));
                protocols = protocols.Where(p => p.UpdatedDate >= DateTime.UtcNow.AddDays(-42));
            }
            else
            {
                protocols = null;
            }

            return protocols.OrderByDescending(p => p.Id).ToList();
        }

        public ICollection<Protocol> GetProtocolsByOrganization(long organizationId, QueryObject dateQuery, ProtocolSearchObject protocolSearch, ClaimsPrincipal claimUser)
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
                protocols = protocols.Where(o => o.Name.ToLower().Contains(protocolSearch.Name.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ReviewComment))
            {
                protocols = protocols.Where(o => o.ReviewComment.ToLower().Contains(protocolSearch.ReviewComment.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ProtocolContent))
            {
                protocols = protocols.Where(o => o.ProtocolContent.Content.ToLower().Contains(protocolSearch.ProtocolContent.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OwnerName))
            {
                protocols = protocols.Where(o => (o.User.FirstName + " " + o.User.LastName).ToLower().Contains(protocolSearch.OwnerName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OrganizationName))
            {
                protocols = protocols.Where(o => o.Organization.Name.ToLower().Contains(protocolSearch.OrganizationName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.UserName))
            {
                protocols = protocols.Where(o => o.AdditionalUser.Select(u => u.User.FirstName.ToLower() + " " + u.User.LastName.ToLower()).Contains(protocolSearch.UserName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.IsClosed.ToString()))
            {
                protocols = protocols.Where(o => o.IsClosed == protocolSearch.IsClosed);
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.IsDraft.ToString()))
            {
                protocols = protocols.Where(o => o.IsDraft == protocolSearch.IsDraft);
            }

            var claimRoles = claimUser.GetRoles();
            var claimOrganizationIds = claimUser.GetOrganizationIds();
            var claimUserId = claimUser.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId == claimUserId).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId || additionalUserProtocolIds.Contains(p.Id));
                protocols = protocols.Where(p => p.UpdatedDate >= DateTime.UtcNow.AddDays(-42));
            }
            else
            {
                protocols = null;
            }

            return protocols.OrderByDescending(p => p.Id).ToList();
        }

        public ICollection<Protocol> GetProtocolsByTemplate(long templateId, QueryObject dateQuery, ProtocolSearchObject protocolSearch, ClaimsPrincipal claimUser)
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
                protocols = protocols.Where(o => o.Name.ToLower().Contains(protocolSearch.Name.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ReviewComment))
            {
                protocols = protocols.Where(o => o.ReviewComment.ToLower().Contains(protocolSearch.ReviewComment.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ProtocolContent))
            {
                protocols = protocols.Where(o => o.ProtocolContent.Content.ToLower().Contains(protocolSearch.ProtocolContent.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OwnerName))
            {
                protocols = protocols.Where(o => (o.User.FirstName + " " + o.User.LastName).ToLower().Contains(protocolSearch.OwnerName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OrganizationName))
            {
                protocols = protocols.Where(o => o.Organization.Name.ToLower().Contains(protocolSearch.OrganizationName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.UserName))
            {
                protocols = protocols.Where(o => o.AdditionalUser.Select(u => u.User.FirstName.ToLower() + " " + u.User.LastName.ToLower()).Contains(protocolSearch.UserName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.IsClosed.ToString()))
            {
                protocols = protocols.Where(o => o.IsClosed == protocolSearch.IsClosed);
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.IsDraft.ToString()))
            {
                protocols = protocols.Where(o => o.IsDraft == protocolSearch.IsDraft);
            }

            var claimRoles = claimUser.GetRoles();
            var claimOrganizationIds = claimUser.GetOrganizationIds();
            var claimUserId = claimUser.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId == claimUserId).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId || additionalUserProtocolIds.Contains(p.Id));
                protocols = protocols.Where(p => p.UpdatedDate >= DateTime.UtcNow.AddDays(-42));
            }
            else
            {
                protocols = null;
            }

            return protocols.OrderByDescending(p => p.Id).ToList();
        }

        public ICollection<Protocol> GetProtocolsByUser(long userId, QueryObject dateQuery, ProtocolSearchObject protocolSearch, ClaimsPrincipal claimUser)
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
                protocols = protocols.Where(o => o.Name.ToLower().Contains(protocolSearch.Name.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ReviewComment))
            {
                protocols = protocols.Where(o => o.ReviewComment.ToLower().Contains(protocolSearch.ReviewComment.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.ProtocolContent))
            {
                protocols = protocols.Where(o => o.ProtocolContent.Content.ToLower().Contains(protocolSearch.ProtocolContent.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OwnerName))
            {
                protocols = protocols.Where(o => (o.User.FirstName + " " + o.User.LastName).ToLower().Contains(protocolSearch.OwnerName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.OrganizationName))
            {
                protocols = protocols.Where(o => o.Organization.Name.ToLower().Contains(protocolSearch.OrganizationName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.UserName))
            {
                protocols = protocols.Where(o => o.AdditionalUser.Select(u => u.User.FirstName.ToLower() + " " + u.User.LastName.ToLower()).Contains(protocolSearch.UserName.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.IsClosed.ToString()))
            {
                protocols = protocols.Where(o => o.IsClosed == protocolSearch.IsClosed);
            }

            if(!string.IsNullOrWhiteSpace(protocolSearch.IsDraft.ToString()))
            {
                protocols = protocols.Where(o => o.IsDraft == protocolSearch.IsDraft);
            }

            var claimRoles = claimUser.GetRoles();
            var claimOrganizationIds = claimUser.GetOrganizationIds();
            var claimUserId = claimUser.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId == claimUserId).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId || additionalUserProtocolIds.Contains(p.Id));
                protocols = protocols.Where(p => p.UpdatedDate >= DateTime.UtcNow.AddDays(-42));
            }
            else
            {
                protocols = null;
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

        public bool UpdateProtocol(long closingUserId, List<long> additionalUserIds, Protocol protocol)
        {
            var oldProtocolData = _context.Protocols.AsNoTracking().FirstOrDefault(p => p.Id == protocol.Id);

            if (oldProtocolData.IsClosed)
            {
                throw new InvalidOperationException("Cannot update a closed protocol.");
            }

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
            
            var closingUser = _context.Users.Where(u => u.Id == closingUserId).FirstOrDefault();

            if (oldProtocolData.IsClosed == false && protocol.IsClosed == true)
            {
                var closedUserMessage = new UserMessage()
                {
                    Subject = "Protokoll wurde überprüft und bestätigt.",
                    MessageContent = "Das versendete Protokoll " + protocol.Name + " vom " + protocol.CreatedDate.ToString("dd.MM.yyyy") + " wurde von " + closingUser.Username + " überprüft und bestätigt.",
                    ReferenceObject = "Protocol",
                    ReferenceObjectId = protocol.Id,
                    SentAt = DateTime.UtcNow,
                    SentFrom = closingUser.Username,
                    IsRead = false,
                    IsArchived = false,
                    userId = protocol.User.Id,
                    User = protocol.User,
                };

                _context.Add(closedUserMessage);
            }
            else if (protocol.IsClosed == false && oldProtocolData.IsDraft == false && protocol.IsDraft == true)
            {
                var errorUserMessage = new UserMessage()
                {
                    Subject = "Protokoll enthält Fehler.",
                    MessageContent = "Das versendete Protokoll " + protocol.Name + " vom " + protocol.CreatedDate.ToString("dd.MM.yyyy") + " wurde von " + closingUser.Username + " überprüft und enthält Fehler." ,
                    ReferenceObject = "Protocol",
                    ReferenceObjectId = protocol.Id,
                    SentAt = DateTime.UtcNow,
                    SentFrom = closingUser.Username,
                    IsRead = false,
                    IsArchived = false,
                    userId = protocol.User.Id,
                    User = protocol.User
                };

                _context.Add(errorUserMessage);
            }
            
            _context.Update(protocol);

            return Save();
        }
    }
}