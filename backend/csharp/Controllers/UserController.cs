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
        [Route("/api/users")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] UserSearchObject userSearchQuery = null)
        {
            var query = _userRepository.GetUsers(dateQuery, userSearchQuery).AsQueryable();

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
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(long id)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userRepository.GetUser(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        // GET: api/user/{id}/organizations
        [HttpGet("{id}/organizations")]
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

        // GET: api/user/{id}/protocols
        [HttpGet]
        [Route("/api/user/{id}/creator-protocols")]
        [ProducesResponseType(200, Type = typeof(ICollection<Protocol>))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocolsByUser(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] ProtocolSearchObject protocolSearchQuery = null)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var query = _protocolRepository.GetProtocolsByUser(id, dateQuery, protocolSearchQuery).AsQueryable();

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
        [Route("/api/user/{id}/viewer-protocols")]
        [ProducesResponseType(200, Type = typeof(ICollection<Protocol>))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocolsByAdditionalUser(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] ProtocolSearchObject protocolSearchQuery = null)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var query = _protocolRepository.GetProtocolsByAdditionalUser(id, dateQuery, protocolSearchQuery).AsQueryable();

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
        [Route("/api/user/{id}/all-protocols")]
        [ProducesResponseType(200, Type = typeof(ICollection<Protocol>))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocolsByUserAndAdditionalUser(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] ProtocolSearchObject protocolSearchQuery = null)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var queryShared = _protocolRepository.GetProtocolsByUser(id, dateQuery, protocolSearchQuery).ToList().AsQueryable();
            var queryOwned = _protocolRepository.GetProtocolsByAdditionalUser(id, dateQuery, protocolSearchQuery).ToList().AsQueryable();

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
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserMessage>))]
        public IActionResult GetMessagesByUser(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] UserMessageSearchObject userMessageSearchQuery = null)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

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
        [ProducesResponseType(200, Type = typeof(int))]
        public IActionResult GetNumberOfProtocolsPerUser(long id)
        {
            var numberOfUnreadMessagesPerUser = _userMessageRepository.GetNumberOfUnreadMessagesPerUser(id);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfUnreadMessagesPerUser);
        }

        // GET: api/users/{id}/organizations/{organizationId}/roles
        [HttpGet("{id}/organizations/{organizationId}/roles")]
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
        [Route("/api/users")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserRegisterDto userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            var userMail = _userRepository.GetUsers(new QueryObject(), new UserSearchObject())
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
            
            userMap.Password = BCrypt.Net.BCrypt.HashPassword(userCreate.Password); // SALT is created automatically by the method.
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

            return Ok("Successfully created.");
        }

        // POST: api/user/{id}/organizations/{organizationId}/role
        [HttpPost("{id}/organization/{organizationId}/role/{roleId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUserOrganizationRole(long id, long organizationId, long roleId)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            if(!_organizationRepository.OrganizationExists(organizationId))
                return NotFound();

            if(!_roleRepository.RoleExists(roleId))
                return NotFound();
                
            if(_userRepository.UserOrganizationRoleExists(id, organizationId, roleId) == true)
            {
                ModelState.AddModelError("", "User Role in this Organization already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var uor = new UserOrganizationRole
            {
                userId = id,
                User = _userRepository.GetUser(id),
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

            return Ok("Successfully created.");
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(long id)
        {
            if (!_userRepository.UserExists(id))
            {
                return NotFound();
            }

            var userMessagesToDelete = _userMessageRepository.GetMessagesByUser(id, new QueryObject(), new UserMessageSearchObject());
            var userToDelete = _userRepository.GetUser(id);
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

        // DELETE: api/user/{id}/organization/{organizationId}/role/{roleId}
        [HttpDelete("{id}/organization/{organizationId}/role/{roleId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(long id, long organizationId, long roleId)
        {
            if (!_userRepository.UserOrganizationRoleExists(id, organizationId, roleId))
            {
                return NotFound();
            }

            var userOrganizationRole = _userRepository.GetUserOrganizationRole(id, organizationId, roleId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.DeleteUserOrganizationRole(userOrganizationRole))
            {
                ModelState.AddModelError("", "Something went wrong when deleting User Role.");
            }

            return NoContent();
        }

        // UPDATE: api/user/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(long id, [FromBody] UserRegisterDto userUpdate)
        {
            if (userUpdate == null)
                return BadRequest(ModelState);

            var userMail = _userRepository.GetUsers(new QueryObject(), new UserSearchObject())
                .Where(o => o.Email.Trim().ToLower() == userUpdate.Email.TrimEnd().ToLower() && o.Id != id)
                .FirstOrDefault();

            if(userMail != null)
            {
                ModelState.AddModelError("", "User E-Mail already exists.");
                return StatusCode(422, ModelState);
            }

            if (!_userRepository.UserExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var userMap = _mapper.Map<User>(userUpdate);
            
            userMap.Id = id;
            userMap.Password = BCrypt.Net.BCrypt.HashPassword(userUpdate.Password); // SALT is created automatically by the method.
            userMap.Email = userUpdate.Email.ToLower();

            var _userPasswordCheck = _userRepository.GetUser(id);

            if(!_userRepository.VerifyUserPassword(_userPasswordCheck, userUpdate.Password))
            {
                if(!_emailService.SendPasswordUpdateEmail(userUpdate.FirstName + " " + userUpdate.LastName, userUpdate.Email, userUpdate.Password))
                {
                    ModelState.AddModelError("", "Something went wrong sending the mail.");
                    return StatusCode(301, ModelState);
                }
            }

            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating User.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}