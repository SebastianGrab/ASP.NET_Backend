using System.Security.Claims;
using Data;
using Helper;
using Helper.SearchObjects;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ProtocolContext _context;
        
        public UserRepository(ProtocolContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            Save();

            var newUserMessage = new UserMessage
            {
                Subject = "Bitte ändern Sie Ihr Passwort.",
                MessageContent = "Ihr initiales Passwort wurde nicht von Ihnen festgelegt. Bitte ändern Sie dieses.",
                ReferenceObject = "User",
                ReferenceObjectId = user.Id,
                SentAt = DateTime.UtcNow,
                SentFrom = "System",
                IsRead = false,
                IsArchived = false,
                User = user
            };

            _context.Add(newUserMessage);
            Save();

            var newUserLoginAttempt = new UserLoginAttempt
            {
                userId = user.Id,
                User = user
            };

            _context.Add(newUserLoginAttempt);
            return Save();
        }

        public bool CreateUserOrganizationRole(UserOrganizationRole userOrganizationRole)
        {
            _context.Add(userOrganizationRole);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public bool DeleteUserOrganizationRole(UserOrganizationRole userOrganizationRole)
        {
            _context.Remove(userOrganizationRole);
            return Save();
        }

        public bool DeleteUserOrganizationRoles(List<UserOrganizationRole> userOrganizationRoles)
        {
            _context.RemoveRange(userOrganizationRoles);
            return Save();
        }

        public ICollection<User> GetAdditionalUsersByProtocol(long protocolId, QueryObject dateQuery, UserSearchObject userSearch)
        {
            var users = _context.AdditionalUsers.Where(au => au.Protocol.Id == protocolId).Select(u => u.User).OrderByDescending(u => u.Id).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                users = users.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                users = users.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                users = users.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                users = users.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(userSearch.Email))
            {
                users = users.Where(o => o.Email.ToLower().Contains(userSearch.Email.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(userSearch.Username))
            {
                users = users.Where(o => (o.FirstName + " " + o.LastName).ToLower().Contains(userSearch.Username.ToLower()));
            }

            return users.OrderByDescending(p => p.Id).ToList();
        }

        public ICollection<UserOrganizationRole> GetUserOrganizationRoleEntriesByUser(long id)
        {
            return _context.UserOrganizationRoles.Where(au => au.User.Id == id).ToList();
        }

        public User GetUser(long id, ClaimsPrincipal claimUser)
        {
            var users = _context.Users.Where(u => u.Id == id).ToList();
            
            var claimRoles = claimUser.GetRoles();
            var claimOrganizationIds = claimUser.GetOrganizationIds();
            var claimUserId = claimUser.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                users = users;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                users = users.Where(u => u.UserOrganizationRoles.Any(uor => claimOrganizationIds.Contains(uor.organizationId)) || u.UserOrganizationRoles.Count() == 0).ToList();
            }
            else if (claimRoles.Contains("Helfer"))
            {
                var userOrga = _context.UserOrganizationRoles.Where(uor => uor.userId == claimUserId).Select(uor => uor.organizationId).ToList();
                users = users.Where(u => u.UserOrganizationRoles.Any(uor => userOrga.Contains(uor.organizationId))).ToList();
            }
            else
            {
                users = null;
            }

            return users.FirstOrDefault();
        }

        public User GetUserByProtocol(long protocolId, ClaimsPrincipal claimUser)
        {
            var users = _context.Protocols.Where(p => p.Id == protocolId).Select(u => u.User).ToList();
            
            var claimRoles = claimUser.GetRoles();
            var claimOrganizationIds = claimUser.GetOrganizationIds();
            var claimUserId = claimUser.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                users = users;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                users = users.Where(u => u.UserOrganizationRoles.Any(uor => claimOrganizationIds.Contains(uor.organizationId)) || u.UserOrganizationRoles.Count() == 0).ToList();
            }
            else if (claimRoles.Contains("Helfer"))
            {
                var userOrga = _context.UserOrganizationRoles.Where(uor => uor.userId == claimUserId).Select(uor => uor.organizationId).ToList();
                users = users.Where(u => u.UserOrganizationRoles.Any(uor => userOrga.Contains(uor.organizationId))).ToList();
            }
            else
            {
                users = null;
            }

            return users.FirstOrDefault();
        }

        public ICollection<User> GetUsers(QueryObject dateQuery, UserSearchObject userSearch, ClaimsPrincipal claimUser)
        {
            var users = _context.Users.OrderByDescending(u => u.Id).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                users = users.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                users = users.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                users = users.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                users = users.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(userSearch.Email))
            {
                users = users.Where(o => o.Email.ToLower().Contains(userSearch.Email.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(userSearch.Username))
            {
                users = users.Where(o => (o.FirstName + " " + o.LastName).ToLower().Contains(userSearch.Username.ToLower()));
            }
            
            var claimRoles = claimUser.GetRoles();
            var claimOrganizationIds = claimUser.GetOrganizationIds();
            var claimUserId = claimUser.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                users = users;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                users = users.Where(u => u.UserOrganizationRoles.Any(uor => claimOrganizationIds.Contains(uor.organizationId)) || u.UserOrganizationRoles.Count() == 0).AsQueryable();
            }
            else if (claimRoles.Contains("Helfer"))
            {
                var userOrga = _context.UserOrganizationRoles.Where(uor => uor.userId == claimUserId).Select(uor => uor.organizationId).ToList();
                users = users.Where(u => u.UserOrganizationRoles.Any(uor => userOrga.Contains(uor.organizationId))).AsQueryable();
            }
            else
            {
                users = null;
            }

            return users.OrderByDescending(p => p.Id).ToList();
        }

        public ICollection<User> GetUsersByOrganization(long organizationId, QueryObject dateQuery, UserSearchObject userSearch, ClaimsPrincipal claimUser)
        {
            var users = _context.UserOrganizationRoles.Where(uor => uor.Organization.Id == organizationId).Select(u => u.User).OrderByDescending(u => u.Id).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                users = users.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                users = users.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                users = users.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                users = users.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(userSearch.Email))
            {
                users = users.Where(o => o.Email.ToLower().Contains(userSearch.Email.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(userSearch.Username))
            {
                users = users.Where(o => (o.FirstName + " " + o.LastName).ToLower().Contains(userSearch.Username.ToLower()));
            }
            
            var claimRoles = claimUser.GetRoles();
            var claimOrganizationIds = claimUser.GetOrganizationIds();
            var claimUserId = claimUser.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                users = users;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                users = users.Where(u => u.UserOrganizationRoles.Any(uor => claimOrganizationIds.Contains(uor.organizationId)) || u.UserOrganizationRoles.Count() == 0).AsQueryable();
            }
            else if (claimRoles.Contains("Helfer"))
            {
                var userOrga = _context.UserOrganizationRoles.Where(uor => uor.userId == claimUserId).Select(uor => uor.organizationId).ToList();
                users = users.Where(u => u.UserOrganizationRoles.Any(uor => userOrga.Contains(uor.organizationId))).AsQueryable();
            }
            else
            {
                users = null;
            }

            return users.OrderByDescending(p => p.Id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool UserExists(long id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        public bool UserOrganizationRoleExists(long userId, long organizationId, long roleId)
        {
            return _context.UserOrganizationRoles.Any(u => u.User.Id == userId && u.Organization.Id == organizationId && u.Role.Id == roleId);
        }

        public UserOrganizationRole GetUserOrganizationRole(long userId, long organizationId, long roleId)
        {
            return _context.UserOrganizationRoles.Where(u => u.User.Id == userId && u.Organization.Id == organizationId && u.Role.Id == roleId).FirstOrDefault();
        }

        public bool VerifyUserPassword(User user, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.Where(u => u.Email.ToLower() == email.ToLower()).FirstOrDefault();
        }

        public List<User> GetUsersByOrganizationAndRole(long organizationId, long roleId, ClaimsPrincipal claimUser)
        {
            var users = _context.UserOrganizationRoles.Where(au => au.organizationId == organizationId && au.roleId == roleId).Select(au => au.User).ToList();

            var claimRoles = claimUser.GetRoles();
            var claimOrganizationIds = claimUser.GetOrganizationIds();
            var claimUserId = claimUser.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                users = users;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                users = users.Where(u => u.UserOrganizationRoles.Any(uor => claimOrganizationIds.Contains(uor.organizationId)) || u.UserOrganizationRoles.Count() == 0).ToList();
            }
            else if (claimRoles.Contains("Helfer"))
            {
                var userOrga = _context.UserOrganizationRoles.Where(uor => uor.userId == claimUserId).Select(uor => uor.organizationId).ToList();
                users = users.Where(u => u.UserOrganizationRoles.Any(uor => userOrga.Contains(uor.organizationId))).ToList();
            }
            else
            {
                users = null;
            }

            return users;
        }

        public bool UserMailExists(long id, string email)
        {
            var userMailCount = _context.Users.Where(o => o.Email.ToLower() == email.ToLower() && o.Id != id).Count();

            return userMailCount > 0;
        }

        public User GetUserAsNoTracking(long id)
        {
            return _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
        }

        public bool UserOrganizationExists(long userId, long organizationId)
        {
            return _context.UserOrganizationRoles.Any(u => u.User.Id == userId && u.Organization.Id == organizationId);
        }
    }
}