using Models;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using AutoMapper;
using Dto;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace backend.Controllers
{
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
        private readonly IMapper _mapper;

        public UserController(IOrganizationRepository organizationRepository, IProtocolRepository protocolRepository, IRoleRepository roleRepository, ITemplateRepository templateRepository, IUserRepository userRepository, IUserMessageRepository userMessageRepository, IAdditionalUserRepository additionalUserRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _protocolRepository = protocolRepository;
            _roleRepository = roleRepository;
            _templateRepository = templateRepository;
            _userMessageRepository = userMessageRepository;
            _additionalUserRepository = additionalUserRepository;
            _mapper = mapper;
        }

        // GET: api/users
        [HttpGet]
        [Route("/api/users")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
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
        public IActionResult GetOrganizationsByUser(long id)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var organizations = _mapper.Map<List<OrganizationDto>>(_organizationRepository.GetOrganizationsByUser(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(organizations.DistinctBy(o => o.Id));
        }

        // GET: api/user/{id}/protocols
        [HttpGet]
        [Route("/api/user/{id}/creator-protocols")]
        [ProducesResponseType(200, Type = typeof(ICollection<Protocol>))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocolsByUser(long id)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var protocols = _mapper.Map<List<ProtocolDto>>(_protocolRepository.GetProtocolsByUser(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(protocols);
        }

        // GET: api/user/{id}/viewer-protocols
        [HttpGet]
        [Route("/api/user/{id}/viewer-protocols")]
        [ProducesResponseType(200, Type = typeof(ICollection<Protocol>))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocolsByAdditionalUser(long id)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var protocols = _mapper.Map<List<ProtocolDto>>(_protocolRepository.GetProtocolsByAdditionalUser(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(protocols);
        }

        // GET: /api/user/{id}/all-protocols
        [HttpGet]
        [Route("/api/user/{id}/all-protocols")]
        [ProducesResponseType(200, Type = typeof(ICollection<Protocol>))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocolsByUserAndAdditionalUser(long id)
        {
            if(!_userRepository.UserExists(id))
                return NotFound();

            var protocolsUser = _mapper.Map<List<ProtocolDto>>(_protocolRepository.GetProtocolsByUser(id));
            var protocolsAdditionalUser = _mapper.Map<List<ProtocolDto>>(_protocolRepository.GetProtocolsByAdditionalUser(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Enumerable.Concat(protocolsUser, protocolsAdditionalUser).ToList().DistinctBy(p => p.Id));
        }

        // GET: api/user/{id}/roles
        [HttpGet("{id}/roles")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetRolesByUser(long id)
        {
            var roles = _mapper.Map<List<RoleDto>>(_roleRepository.GetRolesByUser(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(roles.DistinctBy(o => o.Id));
        }

        // GET: api/user/{id}/messages
        [HttpGet("{id}/messages")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserMessage>))]
        public IActionResult GetMessagesByUser(long id)
        {
            var messages = _mapper.Map<List<UserMessageDto>>(_userMessageRepository.GetMessagesByUser(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(messages);
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
        public IActionResult CreateUser([FromBody] UserDto userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            var userMail = _userRepository.GetUsers()
                .Where(o => o.Email.Trim().ToUpper() == userCreate.Email.TrimEnd().ToUpper())
                .FirstOrDefault();

            if(userMail != null)
            {
                ModelState.AddModelError("", "User E-Mail already exists.");
                return StatusCode(422, ModelState);
            }

            var userId = _userRepository.GetUsers()
                .Where(o => o.Id == userCreate.Id)
                .FirstOrDefault();

            if(userId != null)
            {
                ModelState.AddModelError("", "User Id is already in use.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userCreate);

            if(!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created.");
        }

        // POST: api/user/{id}/organizations/{organizationId}/role
        [HttpPost("{id}/organization/{organizationId}/role/{roleId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUserOrganizationRole(long id, long organizationId, long roleId)
        {
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

            var userMessagesToDelete = _userMessageRepository.GetMessagesByUser(id);
            var userToDelete = _userRepository.GetUser(id);
            var userOrganizationRolesToDelete = _userRepository.GetUserOrganizationRoleEntriesByUser(id);
            var additionalUsersToDelete = _additionalUserRepository.GetAdditionalUserEntriesByUser(id);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userMessageRepository.DeleteUserMessages(userMessagesToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting User Messages.");
            }

            if (!_userRepository.DeleteUserOrganizationRoles(userOrganizationRolesToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting User User Roles.");
            }

            if (!_additionalUserRepository.DeleteAdditionalUserEntries(additionalUsersToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting User as Additional User.");
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

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(long id, [FromBody] UserDto userUpdate)
        {
            if (userUpdate == null)
                return BadRequest(ModelState);

            if (id != userUpdate.Id)
                return BadRequest(ModelState);

            if (!_userRepository.UserExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var userMap = _mapper.Map<User>(userUpdate);

            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating User.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}