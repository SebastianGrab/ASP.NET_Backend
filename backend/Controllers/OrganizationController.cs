using Models;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using AutoMapper;
using Dto;
using Helper;
using Helper.SeachObjects;
using Helper.SearchObjects;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Authorize]
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
        public IActionResult GetOrganizations([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] OrganizationSearchObject organizationSearchQuery = null)
        {
            var query = _organizationRepository.GetOrganizations(dateQuery, organizationSearchQuery).AsQueryable();

            var mappedQuery = _mapper.Map<List<OrganizationDto>>(query.ToList()).AsQueryable();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var paginatedList = PaginatedList<OrganizationDto>.Create(mappedQuery, pageIndex, pageSize);

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
        public IActionResult GetProtocolsByOrganization(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] ProtocolSearchObject protocolSearch = null)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var query = _protocolRepository.GetProtocolsByOrganization(id, dateQuery, protocolSearch).AsQueryable();

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

        // GET: api/organization/{id}/roles
        [HttpGet("{id}/roles")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetRolesByOrganization(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var query = _roleRepository.GetRolesByOrganization(id).AsQueryable();

            var mappedQuery = _mapper.Map<List<RoleDto>>(query.ToList()).AsQueryable();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var paginatedList = PaginatedList<RoleDto>.Create(mappedQuery, pageIndex, pageSize);

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

        // GET: api/organization/{id}/all-templates
        [HttpGet("{id}/templates")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Template>))]
        public IActionResult GetAllTemplatesByOrganization(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] TemplateSearchObject templateSearchQuery = null)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();
                
            var queryShared = _templateRepository.GetSharedTemplatesByOrganization(id, dateQuery, templateSearchQuery).ToList().AsQueryable();
            var queryOwned = _templateRepository.GetTemplatesOwnedByOrganization(id, dateQuery, templateSearchQuery).ToList().AsQueryable();

            var templatesShared = _mapper.Map<List<TemplateDto>>(queryShared).AsQueryable();
            var templatesOwned = _mapper.Map<List<TemplateDto>>(queryOwned).AsQueryable();

            var queryMapped = Enumerable.Concat(templatesShared, templatesOwned).ToList().DistinctBy(p => p.Id).AsQueryable();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var paginatedList = PaginatedList<TemplateDto>.Create(queryMapped, pageIndex, pageSize);

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

        // GET: api/organization/{id}/shared-templates
        [HttpGet("{id}/shared-templates")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Template>))]
        public IActionResult GetSharedTemplatesByOrganization(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] TemplateSearchObject templateSearchQuery = null)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var query = _templateRepository.GetSharedTemplatesByOrganization(id, dateQuery, templateSearchQuery).AsQueryable();

            var mappedQuery = _mapper.Map<List<TemplateDto>>(query.ToList()).AsQueryable();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var paginatedList = PaginatedList<TemplateDto>.Create(mappedQuery, pageIndex, pageSize);

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

        // GET: api/organization/{id}/owning-templates
        [HttpGet("{id}/owning-templates")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Template>))]
        public IActionResult GetTemplatesOwnedByOrganization(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] TemplateSearchObject templateSearchQuery = null)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var query = _templateRepository.GetTemplatesOwnedByOrganization(id, dateQuery, templateSearchQuery).AsQueryable();

            var mappedQuery = _mapper.Map<List<RoleDto>>(query.ToList()).AsQueryable();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var paginatedList = PaginatedList<RoleDto>.Create(mappedQuery, pageIndex, pageSize);

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

        // GET: api/organization/{id}/users
        [HttpGet("{id}/users")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsersByOrganization(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] UserSearchObject userSearchQuery = null)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var query = _userRepository.GetUsersByOrganization(id, dateQuery, userSearchQuery).AsQueryable();

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

        // GET: api/organization/{id}/users/{userId}/roles
        [HttpGet("{id}/users/{userId}/roles")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetRolesByUserAndOrganization(long userId, long id)
        {
            if(!_userRepository.UserExists(userId))
                return NotFound();

            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();
                
            var roles = _mapper.Map<List<RoleDto>>(_roleRepository.GetRolesByUserAndOrganization(userId, id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(roles);
        }

        // GET: api/organization/{id}/daughter-organizations
        [HttpGet("{id}/daughter-organizations")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Organization>))]
        public IActionResult GetOrganizationDaughters(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] OrganizationSearchObject organizationSearch = null)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var query = _organizationRepository.GetOrganizationDaughters(id, dateQuery, organizationSearch).AsQueryable();

            var mappedQuery = _mapper.Map<List<OrganizationDto>>(query.ToList()).AsQueryable();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var paginatedList = PaginatedList<OrganizationDto>.Create(mappedQuery, pageIndex, pageSize);

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

        // POST: api/organizations
        [HttpPost]
        [Route("/api/organizations")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOrganization([FromBody] OrganizationDto organizationCreate)
        {
            if (organizationCreate == null)
                return BadRequest(ModelState);

            var organizationName = _organizationRepository.GetOrganizations(new QueryObject(), new OrganizationSearchObject())
                .Where(o => o.Name.Trim().ToUpper() == organizationCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if(organizationName != null)
            {
                ModelState.AddModelError("", "Organization already exists.");
                return StatusCode(422, ModelState);
            }

            var organizationId = _organizationRepository.GetOrganizations(new QueryObject(), new OrganizationSearchObject())
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
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();
                
            if(!_userRepository.UserExists(userId))
                return NotFound();
                
            if(!_roleRepository.RoleExists(roleId))
                return NotFound();
                
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
            var templateOrganizationsToDelete = _templateOrganizationRepository.GetTemplateOrganizationEntriesByOrganization(id).ToList();
            var userOrganizationRolesToDelete = _organizationRepository.GetUserOrganizationRoleEntriesByOrganization(id).ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_templateOrganizationRepository.DeleteTemplateOrganizationEntries(templateOrganizationsToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting template organization.");
            }

            if (!_organizationRepository.DeleteOrganization(organizationToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting Organization.");
            }

            if (!_userRepository.DeleteUserOrganizationRoles(userOrganizationRolesToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting Organization Roles.");
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

            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();   

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
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();
                
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