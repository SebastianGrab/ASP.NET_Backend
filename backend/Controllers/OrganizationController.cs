using Models;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using AutoMapper;
using Dto;

namespace backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IProtocolRepository _protocolRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITemplateOrganizationRepository _templateOrganizationRepository;
        private readonly IMapper _mapper;

        public OrganizationController(IOrganizationRepository organizationRepository, IProtocolRepository protocolRepository, IRoleRepository roleRepository, ITemplateRepository templateRepository, IUserRepository userRepository, ITemplateOrganizationRepository templateOrganizationRepository, IMapper mapper)
        {
            _organizationRepository = organizationRepository;
            _protocolRepository = protocolRepository;
            _roleRepository = roleRepository;
            _templateRepository = templateRepository;
            _userRepository = userRepository;
            _templateOrganizationRepository = templateOrganizationRepository;
            _mapper = mapper;
        }

        // GET: api/organizations
        [HttpGet]
        [Route("/api/organizations")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Organization>))]
        public IActionResult GetOrganizations()
        {
            var organizations = _mapper.Map<List<OrganizationDto>>(_organizationRepository.GetOrganizations());

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(organizations);
        }

        // GET: api/organization/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Organization))]
        [ProducesResponseType(400)]
        public IActionResult GetOrganization(long id)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var organization = _mapper.Map<OrganizationDto>(_organizationRepository.GetOrganization(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(organization);
        }

        // GET: api/organization/{id}/protocols
        [HttpGet("{id}/protocols")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Protocol>))]
        public IActionResult GetProtocolsByOrganization(long id)
        {
            var protocols = _mapper.Map<List<ProtocolDto>>(_protocolRepository.GetProtocolsByOrganization(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(protocols);
        }

        // GET: api/organization/{id}/roles
        [HttpGet("{id}/roles")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetRolesByOrganization(long id)
        {
            var roles = _mapper.Map<List<RoleDto>>(_roleRepository.GetRolesByOrganization(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(roles);
        }

        // GET: api/organization/{id}/all-templates
        [HttpGet("{id}/templates")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Template>))]
        public IActionResult GetAllTemplatesByOrganization(long id)
        {
            var templatesShared = _mapper.Map<List<TemplateDto>>(_templateRepository.GetSharedTemplatesByOrganization(id));
            var templatesOwned = _mapper.Map<List<TemplateDto>>(_templateRepository.GetTemplatesOwnedByOrganization(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Enumerable.Concat(templatesShared, templatesOwned).ToList().DistinctBy(p => p.Id));
        }

        // GET: api/organization/{id}/shared-templates
        [HttpGet("{id}/shared-templates")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Template>))]
        public IActionResult GetSharedTemplatesByOrganization(long id)
        {
            var templates = _mapper.Map<List<TemplateDto>>(_templateRepository.GetSharedTemplatesByOrganization(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(templates);
        }

        // GET: api/organization/{id}/owning-templates
        [HttpGet("{id}/owning-templates")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Template>))]
        public IActionResult GetTemplatesOwnedByOrganization(long id)
        {
            var templates = _mapper.Map<List<TemplateDto>>(_templateRepository.GetTemplatesOwnedByOrganization(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(templates);
        }

        // GET: api/organization/{id}/users
        [HttpGet("{id}/users")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsersByOrganization(long id)
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsersByOrganization(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        // GET: api/organization/{id}/users/{userId}/roles
        [HttpGet("{id}/users/{userId}/roles")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetRolesByUserAndOrganization(long userId, long id)
        {
            var roles = _mapper.Map<List<RoleDto>>(_roleRepository.GetRolesByUserAndOrganization(userId, id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(roles);
        }

        // POST: api/organizations
        [HttpPost]
        [Route("/api/organizations")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOrganization([FromBody] OrganizationDto organizationCreate)
        {
            if (organizationCreate == null)
                return BadRequest(ModelState);

            var organizationName = _organizationRepository.GetOrganizations()
                .Where(o => o.Name.Trim().ToUpper() == organizationCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if(organizationName != null)
            {
                ModelState.AddModelError("", "Organization already exists.");
                return StatusCode(422, ModelState);
            }

            var organizationId = _organizationRepository.GetOrganizations()
                .Where(o => o.Id == organizationCreate.Id)
                .FirstOrDefault();

            if(organizationId != null)
            {
                ModelState.AddModelError("", "Organization Id is already in use.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var organizationMap = _mapper.Map<Organization>(organizationCreate);

            if(!_organizationRepository.CreateOrganization(organizationMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created.");
        }

        // POST: api/organizations/{id}/user/{userId}/role
        [HttpPost("{id}/user/{userId}/role/{roleId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUserOrganizationRole(long userId, long id, long roleId)
        {
            if(_userRepository.UserOrganizationRoleExists(userId, id, roleId) == true)
            {
                ModelState.AddModelError("", "User Role in this Organization already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var uor = new UserOrganizationRole
            {
                userId = userId,
                User = _userRepository.GetUser(userId),
                organizationId = id,
                Organization = _organizationRepository.GetOrganization(id),
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

        // DELETE: api/organization/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOrganization(long id)
        {
            if (!_organizationRepository.OrganizationExists(id))
            {
                return NotFound();
            }

            var organizationToDelete = _organizationRepository.GetOrganization(id);
            var templateOrganizationsToDelete = _templateOrganizationRepository.GetTemplateOrganizationEntriesByOrganization(id);
            var userOrganizationRolesToDelete = _organizationRepository.GetUserOrganizationRoleEntriesByOrganization(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_templateOrganizationRepository.DeleteTemplateOrganizationEntries(templateOrganizationsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting template organization.");
            }

            if (!_organizationRepository.DeleteOrganization(organizationToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting Organization.");
            }

            return NoContent();
        }

        // DELETE: api/organization/{id}/user/{userId}/role/{roleId}
        [HttpDelete("{id}/user/{userId}/role/{roleId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(long id, long userId, long roleId)
        {
            if (!_userRepository.UserOrganizationRoleExists(userId, id, roleId))
            {
                return NotFound();
            }

            var userOrganizationRole = _userRepository.GetUserOrganizationRole(userId, id, roleId);

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
        public IActionResult UpdateOrganization(long id, [FromBody] OrganizationDto organizationUpdate)
        {
            if (organizationUpdate == null)
                return BadRequest(ModelState);

            if (id != organizationUpdate.Id)
                return BadRequest(ModelState);

            if (!_organizationRepository.OrganizationExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var organizationMap = _mapper.Map<Organization>(organizationUpdate);

            if (!_organizationRepository.UpdateOrganization(organizationMap))
            {
                ModelState.AddModelError("", "Something went wrong updating organization.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}