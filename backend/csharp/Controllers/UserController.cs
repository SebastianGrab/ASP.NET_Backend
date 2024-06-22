using Models;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using AutoMapper;
using Dto;
using Microsoft.Extensions.Configuration.UserSecrets;
using Helper;
using Helper.SeachObjects;
using Helper.SearchObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IProtocolRepository _protocolRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IUserMessageRepository _userMessageRepository;
        private readonly IAdditionalUserRepository _additionalUserRepository;
        private readonly IUserLoginAttemptRepository _userLoginAttemptRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public UserController(IOrganizationRepository organizationRepository, IProtocolRepository protocolRepository, IRoleRepository roleRepository, ITemplateRepository templateRepository, IUserRepository userRepository, IUserMessageRepository userMessageRepository, IAdditionalUserRepository additionalUserRepository, IUserLoginAttemptRepository userLoginAttemptRepository, IEmailService emailService, IMapper mapper)
        {
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _protocolRepository = protocolRepository;
            _roleRepository = roleRepository;
            _templateRepository = templateRepository;
            _userMessageRepository = userMessageRepository;
            _additionalUserRepository = additionalUserRepository;
            _userLoginAttemptRepository = userLoginAttemptRepository;
            _emailService = emailService;
            _mapper = mapper;
        }

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [Route("/api/users")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] UserSearchObject userSearchQuery = null)
        {
            var query = _userRepository.GetUsers(dateQuery, userSearchQuery, User).AsQueryable();

            var mappedQuery = _mapper.Map<List<UserDto>>(query.ToList()).AsQueryable();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var paginatedList = PaginatedList<UserDto>.Create(mappedQuery, pageIndex, pageSize);

            var response = new
            {
                totalCount = paginatedList.TotalCount,
                totalPages = paginatedList.TotalPages,
                currentPage = paginatedList.PageIndex,
                pageSize = paginatedList.PageSize,
                items = paginatedList
            };

            return Ok(response);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(long id)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userRepository.GetUser(id, User));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        // GET: api/user/{id}/organizations
        [HttpGet("{id}/organizations")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(ICollection<Organization>))]
        [ProducesResponseType(400)]
        public IActionResult GetOrganizationsByUser(long id, [FromQuery] QueryObject dateQuery = null, [FromQuery] OrganizationSearchObject organizationSearchQuery = null)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var organizations = _mapper.Map<List<OrganizationDto>>(_organizationRepository.GetOrganizationsByUser(id, dateQuery, organizationSearchQuery));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(organizations.DistinctBy(o => o.Id));
        }

        // GET: api/user/{id}/creator-protocols
        [HttpGet]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [Route("/api/user/{id}/creator-protocols")]
        [ProducesResponseType(200, Type = typeof(ICollection<Protocol>))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocolsByUser(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] ProtocolSearchObject protocolSearchQuery = null)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var query = _protocolRepository.GetProtocolsByUser(id, dateQuery, protocolSearchQuery, User).AsQueryable();

            var mappedQuery = _mapper.Map<List<ProtocolDto>>(query.ToList()).AsQueryable();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var paginatedList = PaginatedList<ProtocolDto>.Create(mappedQuery, pageIndex, pageSize);

            var response = new
            {
                totalCount = paginatedList.TotalCount,
                totalPages = paginatedList.TotalPages,
                currentPage = paginatedList.PageIndex,
                pageSize = paginatedList.PageSize,
                items = paginatedList
            };

            return Ok(response);
        }

        // GET: api/user/{id}/viewer-protocols
        [HttpGet]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [Route("/api/user/{id}/viewer-protocols")]
        [ProducesResponseType(200, Type = typeof(ICollection<Protocol>))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocolsByAdditionalUser(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] ProtocolSearchObject protocolSearchQuery = null)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var query = _protocolRepository.GetProtocolsByAdditionalUser(id, dateQuery, protocolSearchQuery, User).AsQueryable();

            var mappedQuery = _mapper.Map<List<ProtocolDto>>(query.ToList()).AsQueryable();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var paginatedList = PaginatedList<ProtocolDto>.Create(mappedQuery, pageIndex, pageSize);

            var response = new
            {
                totalCount = paginatedList.TotalCount,
                totalPages = paginatedList.TotalPages,
                currentPage = paginatedList.PageIndex,
                pageSize = paginatedList.PageSize,
                items = paginatedList
            };

            return Ok(response);
        }

        // GET: /api/user/{id}/all-protocols
        [HttpGet]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [Route("/api/user/{id}/all-protocols")]
        [ProducesResponseType(200, Type = typeof(ICollection<Protocol>))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocolsByUserAndAdditionalUser(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] ProtocolSearchObject protocolSearchQuery = null)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var queryShared = _protocolRepository.GetProtocolsByUser(id, dateQuery, protocolSearchQuery, User).ToList().AsQueryable();
            var queryOwned = _protocolRepository.GetProtocolsByAdditionalUser(id, dateQuery, protocolSearchQuery, User).ToList().AsQueryable();

            var protocolsShared = _mapper.Map<List<ProtocolDto>>(queryShared).AsQueryable();
            var protocolsOwned = _mapper.Map<List<ProtocolDto>>(queryOwned).AsQueryable();

            var mappedQuery = Enumerable.Concat(protocolsShared, protocolsOwned).ToList().DistinctBy(p => p.Id).AsQueryable();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var paginatedList = PaginatedList<ProtocolDto>.Create(mappedQuery, pageIndex, pageSize);

            var response = new
            {
                totalCount = paginatedList.TotalCount,
                totalPages = paginatedList.TotalPages,
                currentPage = paginatedList.PageIndex,
                pageSize = paginatedList.PageSize,
                items = paginatedList
            };

            return Ok(response);
        }

        // GET: api/user/{id}/roles
        [HttpGet("{id}/roles")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetRolesByUser(long id)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var roles = _mapper.Map<List<RoleDto>>(_roleRepository.GetRolesByUser(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(roles.DistinctBy(o => o.Id));
        }

        // GET: api/user/{id}/messages
        [HttpGet("{id}/messages")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserMessage>))]
        public IActionResult GetMessagesByUser(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] UserMessageSearchObject userMessageSearchQuery = null)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var userId = User.GetUserId();
            var roles = User.GetRoles();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (userId.ToString().IsNullOrEmpty() || id != userId)
                {
                    return Unauthorized();
                }
            }

            var query = _userMessageRepository.GetMessagesByUser(id, dateQuery, userMessageSearchQuery).AsQueryable();

            var mappedQuery = _mapper.Map<List<UserMessageDto>>(query.ToList()).AsQueryable();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var paginatedList = PaginatedList<UserMessageDto>.Create(mappedQuery, pageIndex, pageSize);

            var response = new
            {
                totalCount = paginatedList.TotalCount,
                totalPages = paginatedList.TotalPages,
                currentPage = paginatedList.PageIndex,
                pageSize = paginatedList.PageSize,
                items = paginatedList
            };

            return Ok(response);
        }

        // GET: api/user/{id}/no-of-unread-messages
        [HttpGet("{id}/no-of-unread-messages")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(int))]
        public IActionResult GetNumberOfProtocolsPerUser(long id)
        {
            var numberOfUnreadMessagesPerUser = _userMessageRepository.GetNumberOfUnreadMessagesPerUser(id);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.GetUserId();
            var roles = User.GetRoles();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (userId.ToString().IsNullOrEmpty() || id != userId)
                {
                    return Unauthorized();
                }
            }

            return Ok(numberOfUnreadMessagesPerUser);
        }

        // GET: api/users/{id}/organizations/{organizationId}/roles
        [HttpGet("{id}/organizations/{organizationId}/roles")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetRolesByUserAndOrganization(long id, long organizationId)
        {
            var roles = _mapper.Map<List<RoleDto>>(_roleRepository.GetRolesByUserAndOrganization(id, organizationId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(roles.DistinctBy(o => o.Id));
        }

        // POST: api/users
        [HttpPost]
        [Authorize(Roles = "Admin,Leiter")]
        [Route("/api/users")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserRegisterDto userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            if(_userRepository.UserExists(userCreate.Id))
            {
                ModelState.AddModelError("", "User Id already exists.");
                return StatusCode(422, ModelState);
            }

            var userMail = _userRepository.GetUsers(new QueryObject(), new UserSearchObject(), User)
                .Where(o => o.Email.Trim().ToLower() == userCreate.Email.TrimEnd().ToLower())
                .FirstOrDefault();

            if(userMail != null)
            {
                ModelState.AddModelError("", "User E-Mail already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userCreate);
            
            userMap.Password = BCrypt.Net.BCrypt.HashPassword(userCreate.Password);
            userMap.Email = userCreate.Email.ToLower();

            if(!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            if(!_emailService.SendRegistrationEmail(userCreate.FirstName + " " + userCreate.LastName, userCreate.Email, userCreate.Password))
            {
                ModelState.AddModelError("", "Saving was successful. Something went wrong sending the mail.");
                return StatusCode(301, ModelState);
            }

            var userToReturn = _mapper.Map<UserDto>(userMap);

            return Ok(userToReturn);
        }

        // POST: api/user/{id}/organizations/{organizationId}/role
        [HttpPost("{id}/organization/{organizationId}/role/{roleId}")]
        [Authorize(Roles = "Admin,Leiter")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUserOrganizationRole(long id, long organizationId, long roleId)
        {
            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();

            var userRole = _roleRepository.GetRole(roleId);

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(organizationId) || userRole.Name == "Admin")
                {
                    return Unauthorized();
                }
            }
            
            if(!_userRepository.UserExists(id))
                return NotFound();

            if(!_organizationRepository.OrganizationExists(organizationId))
                return NotFound();

            if(!_roleRepository.RoleExists(roleId))
                return NotFound();
                
            if(_userRepository.UserOrganizationExists(id, organizationId) == true)
            {
                ModelState.AddModelError("", "User already has a Role in this Organization.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var uor = new UserOrganizationRole
            {
                userId = id,
                User = _userRepository.GetUser(id, User),
                organizationId = organizationId,
                Organization = _organizationRepository.GetOrganization(organizationId),
                roleId = roleId,
                Role = _roleRepository.GetRole(roleId)
            };

            if (!_userRepository.CreateUserOrganizationRole(uor))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully added.");
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(long id)
        {
            if (!_userRepository.UserExists(id))
            {
                return NotFound();
            }

            var roles = User.GetRoles();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (userClaimId.ToString().IsNullOrEmpty() || userClaimId != id)
                {
                    return Unauthorized();
                }
            }

            var userMessagesToDelete = _userMessageRepository.GetMessagesByUser(id, new QueryObject(), new UserMessageSearchObject());
            var userToDelete = _userRepository.GetUser(id, User);
            var userOrganizationRolesToDelete = _userRepository.GetUserOrganizationRoleEntriesByUser(id);
            var additionalUsersToDelete = _additionalUserRepository.GetAdditionalUserEntriesByUser(id);
            var userLoginAttemptToDelete = _userLoginAttemptRepository.GetUserLoginAttempt(id);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (userMessagesToDelete != null)
            {
                if (!_userMessageRepository.DeleteUserMessages(userMessagesToDelete.ToList()))
                {
                    ModelState.AddModelError("", "Something went wrong when deleting User Messages.");
                }
            }

            if (userOrganizationRolesToDelete != null)
            {
                if (!_userRepository.DeleteUserOrganizationRoles(userOrganizationRolesToDelete.ToList()))
                {
                    ModelState.AddModelError("", "Something went wrong when deleting User User Roles.");
                }
            }

            if (additionalUsersToDelete != null)
            {
                if (!_additionalUserRepository.DeleteAdditionalUserEntries(additionalUsersToDelete.ToList()))
                {
                    ModelState.AddModelError("", "Something went wrong when deleting User as Additional User.");
                }
            }

            if (userLoginAttemptToDelete != null)
            {
                if (!_userLoginAttemptRepository.DeleteUserLoginAttempt(userLoginAttemptToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong deleting User Login Attempts.");
                }
            }

            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting User.");
            }

            return NoContent();
        }

        // DELETE: api/user/{id}/organization/{organizationId}
        [HttpDelete("{id}/organization/{organizationId}")]
        [Authorize(Roles = "Admin,Leiter")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(long id, long organizationId)
        {
            if (!_userRepository.UserOrganizationExists(id, organizationId))
            {
                return NotFound();
            }

            var userOrganizationRole = _userRepository.GetUserOrganizationRoleEntriesByUser(id).Where(uor => uor.organizationId == organizationId).ToList();

            var userRole = _roleRepository.GetRole(userOrganizationRole.Select(u => u.roleId).Max());

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(organizationId) || userRole.Name == "Admin")
                {
                    return Unauthorized();
                }
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.DeleteUserOrganizationRoles(userOrganizationRole))
            {
                ModelState.AddModelError("", "Something went wrong when deleting User Role.");
            }

            return NoContent();
        }

        // UPDATE: api/user/{id}/password
        [HttpPut("{id}/password")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUserPassword(long id, [FromBody] UserRegisterDto userUpdate)
        {
            if (!_userRepository.UserExists(id))
                return NotFound();

            var roles = User.GetRoles();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (userClaimId.ToString().IsNullOrEmpty() || userClaimId != id)
                {
                    return Unauthorized();
                }
            }

            if (userUpdate == null)
                return BadRequest(ModelState);

            if(id != userUpdate.Id)
            {
                ModelState.AddModelError("", "IDs do not match.");
                return StatusCode(422, ModelState);
            }

            if(_userRepository.UserMailExists(id, userUpdate.Email))
            {
                ModelState.AddModelError("", "User E-Mail already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var userMap = _mapper.Map<User>(userUpdate);
            
            userMap.Password = BCrypt.Net.BCrypt.HashPassword(userUpdate.Password); // SALT is created automatically by the method.
            userMap.Email = userUpdate.Email.ToLower();

            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating User.");
                return StatusCode(500, ModelState);
            }

            var userToReturn = _mapper.Map<UserDto>(userMap);

            return Ok(userToReturn);
        }

        // UPDATE: api/user/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(long id, [FromBody] UserDto userUpdate)
        {
            if (!_userRepository.UserExists(id))
                return NotFound();

            var userOrgas = _userRepository.GetUserOrganizationRoleEntriesByUser(id).Select(uor => uor.organizationId).ToList();

            var roles = User.GetRoles();
            var userClaimId = User.GetUserId();
            var orgaIds = User.GetOrganizationIds();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (roles.IsNullOrEmpty() || !roles.Contains("Leiter") || !orgaIds.Intersect(userOrgas).Any())
                {
                    if (roles.Contains("Helfer") && id != userClaimId)
                    {
                        return Unauthorized();
                    }
                }
            }

            if (userUpdate == null)
                return BadRequest(ModelState);

            if(id != userUpdate.Id)
            {
                ModelState.AddModelError("", "IDs do not match.");
                return StatusCode(422, ModelState);
            }

            if(_userRepository.UserMailExists(id, userUpdate.Email))
            {
                ModelState.AddModelError("", "User E-Mail already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var userMap = _mapper.Map<User>(userUpdate);
            
            userMap.Password = _userRepository.GetUserAsNoTracking(id).Password;
            userMap.Email = userUpdate.Email.ToLower();

            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating User.");
                return StatusCode(500, ModelState);
            }

            var userToReturn = _mapper.Map<UserDto>(userMap);

            return Ok(userToReturn);
        }
    }
}