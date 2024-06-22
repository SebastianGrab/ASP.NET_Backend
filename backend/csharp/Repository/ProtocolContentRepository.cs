using System.Security.Claims;
using System.Text.Json;
using Data;
using Interfaces;
using Models;
using Newtonsoft.Json.Linq;

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

        public ProtocolContent GetProtocolContent(long protocolId, ClaimsPrincipal claimUser)
        {
            var protocols = _context.Protocols.Where(p => p.Id == protocolId).AsQueryable();

            var claimRoles = claimUser.GetRoles();
            var claimOrganizationIds = claimUser.GetOrganizationIds();
            var claimUserId = claimUser.GetUserId(); 
            var protocolUpdateDate = _context.Protocols.Where(p => p.Id == protocolId).Select(p => p.UpdatedDate).FirstOrDefault();
            var returnProtocolContent = false;

            if (claimRoles.Contains("Admin"))
            {
                returnProtocolContent = true;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                var protocolOrganization = _context.Protocols.Where(p => p.Id == protocolId).Select(p => p.Organization.Id).FirstOrDefault();
                if ( claimOrganizationIds.Contains(protocolOrganization))
                {
                    returnProtocolContent = true;
                }
            }
            else if (claimRoles.Contains("Helfer"))
            {
                var protocolUserId = _context.Protocols.Where(p => p.Id == protocolId).Select(p => p.User.Id).FirstOrDefault();
                var additionalUserIds = _context.AdditionalUsers.Where(au => au.protocolId == protocolId).Select(p => p.userId).ToList();
                if (protocolUserId == claimUserId || additionalUserIds.Contains(claimUserId))
                {
                    returnProtocolContent = true;
                }

                if (protocolUpdateDate < DateTime.UtcNow.AddDays(-42))
                {
                    returnProtocolContent = false;
                }
            }
            else
            {
                returnProtocolContent = false;
            }

            var protocolIdClaimed = protocols.Select(p => p.Id).FirstOrDefault();
            
            if (!returnProtocolContent)
            {
                return null;
            }

            return _context.ProtocolContents.Where(pc => pc.protocolId == protocolId).FirstOrDefault();
        }

        public string GetProtocolNumber(long protocolId)
        {
            var protocolContent = _context.ProtocolContents.Where(p => p.protocolId == protocolId).FirstOrDefault();

            if (protocolContent == null)
            {
                try
                {
                    var content = JObject.Parse(protocolContent.Content);

                    var number = content["Schema"]?
                        .Select(schema => schema["Inputs"])
                        .OfType<JObject>()
                        .Where(input => (string)input["Name"] == "Auftragsnummer")
                        .Select(input => (string)input["Value"]).FirstOrDefault();

                    return number;
                }
                catch (JsonException)
                {
                    return null;
                }
            }
            return null;
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