using System.Security.Claims;
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

        public ProtocolPdfFile GetProtocolPdfFile(long protocolId, ClaimsPrincipal claimUser)
        {
            var protocols = _context.Protocols.Where(p => p.Id == protocolId).AsQueryable();

            var claimRoles = claimUser.GetRoles();
            var claimOrganizationIds = claimUser.GetOrganizationIds();
            var claimUserId = claimUser.GetUserId(); 
            var protocolOrganization = _context.Protocols.Where(p => p.Id == protocolId).Select(p => p.Organization.Id).FirstOrDefault();
            var protocolUserId = _context.Protocols.Where(p => p.Id == protocolId).Select(p => p.User.Id).FirstOrDefault();
            var protocolUpdateDate = _context.Protocols.Where(p => p.Id == protocolId).Select(p => p.UpdatedDate).FirstOrDefault();
            var returnProtocolPdf = false;

            if (claimRoles.Contains("Admin"))
            {
                returnProtocolPdf = true;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                if ( claimOrganizationIds.Contains(protocolOrganization))
                {
                    returnProtocolPdf = true;
                }
            }
            else if (claimRoles.Contains("Helfer"))
            {
                var additionalUserIds = _context.AdditionalUsers.Where(au => au.protocolId == protocolId).Select(p => p.userId).ToList();
                if (protocolUserId == claimUserId || additionalUserIds.Contains(claimUserId))
                {
                    returnProtocolPdf = true;
                }

                if (protocolUpdateDate < DateTime.UtcNow.AddDays(-42))
                {
                    returnProtocolPdf = false;
                }
            }
            else
            {
                returnProtocolPdf = false;
            }

            var protocolIdClaimed = protocols.Select(p => p.Id).FirstOrDefault();
            
            if (!returnProtocolPdf)
            {
                return null;
            }

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