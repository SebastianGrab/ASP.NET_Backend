using Models;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using AutoMapper;
using Dto;
using Helper;
using Helper.SeachObjects;
using Helper.SearchObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

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
        [Authorize(Roles = "Admin,Leiter")]
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
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(Organization))]
        [ProducesResponseType(400)]
        public IActionResult GetOrganization(long id)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (roles.IsNullOrEmpty() || !roles.Contains("Leiter"))
                {
                    if (orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(id))
                    {
                        return Unauthorized();
                    }
                }
            }

            var organization = _mapper.Map<OrganizationDto>(_organizationRepository.GetOrganization(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(organization);
        }

        // GET: api/organization/{id}/protocols
        [HttpGet("{id}/protocols")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Protocol>))]
        public IActionResult GetProtocolsByOrganization(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] ProtocolSearchObject protocolSearch = null)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var query = _protocolRepository.GetProtocolsByOrganization(id, dateQuery, protocolSearch, User).AsQueryable();

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
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetRolesByOrganization(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(id))
                {
                    return Unauthorized();
                }
            }

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

        // GET: api/organization/{id}/templates
        [HttpGet("{id}/templates")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Template>))]
        public IActionResult GetAllTemplatesByOrganization(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] TemplateSearchObject templateSearchQuery = null)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(id))
                {
                    return Unauthorized();
                }
            }
                
            var mothers = _organizationRepository.GetAllOrganizationMothers(id, new QueryObject(), new OrganizationSearchObject()).Select(m => m.Id).ToList();

            var queryShared = _templateRepository.GetSharedTemplatesByOrganization(id, dateQuery, templateSearchQuery).ToList().AsQueryable();
            var queryOwned = _templateRepository.GetTemplatesOwnedByOrganization(id, dateQuery, templateSearchQuery).ToList().AsQueryable();
            // var queryOwnedByMothers = _templateRepository.GetTemplatesOwnedByOrganizations(mothers, dateQuery, templateSearchQuery).ToList().AsQueryable();

            var templatesShared = _mapper.Map<List<TemplateDto>>(queryShared).AsQueryable();
            var templatesOwned = _mapper.Map<List<TemplateDto>>(queryOwned).AsQueryable();
            // var templatesOwnedByMothers = _mapper.Map<List<TemplateDto>>(queryOwnedByMothers).AsQueryable();

            var queryMapped = Enumerable.Concat(templatesOwned, templatesShared).ToList().DistinctBy(p => p.Id).OrderByDescending(p => p.Id).AsQueryable();

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
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Template>))]
        public IActionResult GetSharedTemplatesByOrganization(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] TemplateSearchObject templateSearchQuery = null)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(id))
                {
                    return Unauthorized();
                }
            }

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
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Template>))]
        public IActionResult GetTemplatesOwnedByOrganization(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] TemplateSearchObject templateSearchQuery = null)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(id))
                {
                    return Unauthorized();
                }
            }

            var query = _templateRepository.GetTemplatesOwnedByOrganization(id, dateQuery, templateSearchQuery).AsQueryable();

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

        // GET: api/organization/{id}/users
        [HttpGet("{id}/users")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsersByOrganization(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] UserSearchObject userSearchQuery = null)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(id))
                {
                    return Unauthorized();
                }
            }

            var query = _userRepository.GetUsersByOrganization(id, dateQuery, userSearchQuery, User).AsQueryable();

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
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetRolesByUserAndOrganization(long userId, long id)
        {
            if(!_userRepository.UserExists(userId))
                return NotFound();

            var userRoles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (userRoles.IsNullOrEmpty() || !userRoles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !userRoles.Contains("Helfer") || !orgaIds.Contains(id))
                {
                    if (orgaIds.ToString().IsNullOrEmpty() || userClaimId != userId)
                    {
                        return Unauthorized();
                    }
                }
            }

            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();
                
            var roles = _mapper.Map<List<RoleDto>>(_roleRepository.GetRolesByUserAndOrganization(userId, id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(roles);
        }

        // GET: api/organization/{id}/daughter-organizations
        // [HttpGet("{id}/daughter-organizations")]
        // [Authorize(Roles = "Admin,Leiter,Helfer")]
        // [ProducesResponseType(200, Type = typeof(IEnumerable<Organization>))]
        // public IActionResult GetOrganizationDaughters(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] OrganizationSearchObject organizationSearch = null)
        // {
        //     if(!_organizationRepository.OrganizationExists(id))
        //         return NotFound();

        //     var query = _organizationRepository.GetOrganizationDaughters(id, dateQuery, organizationSearch).AsQueryable();

        //     var mappedQuery = _mapper.Map<List<OrganizationDto>>(query.ToList()).AsQueryable();

        //     if(!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     var paginatedList = PaginatedList<OrganizationDto>.Create(mappedQuery, pageIndex, pageSize);

        //     var response = new
        //     {
        //         totalCount = paginatedList.TotalCount,
        //         totalPages = paginatedList.TotalPages,
        //         currentPage = paginatedList.PageIndex,
        //         pageSize = paginatedList.PageSize,
        //         items = paginatedList
        //     };

        //     return Ok(response);
        // }

        // GET: api/organization/{id}/all-daughter-organizations
        [HttpGet("{id}/all-daughter-organizations")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Organization>))]
        public IActionResult GetAllOrganizationDaughters(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] OrganizationSearchObject organizationSearch = null)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(id))
                {
                    return Unauthorized();
                }
            }

            var query = _organizationRepository.GetAllOrganizationDaughters(id, dateQuery, organizationSearch).AsQueryable();

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

        // GET: api/organization/{id}/all-mother-organizations
        [HttpGet("{id}/all-mother-organizations")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Organization>))]
        public IActionResult GetAllOrganizationMothers(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] OrganizationSearchObject organizationSearch = null)
        {
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(id))
                {
                    return Unauthorized();
                }
            }

            var query = _organizationRepository.GetAllOrganizationMothers(id, dateQuery, organizationSearch).AsQueryable();

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
        [Authorize(Roles = "Admin,Leiter")]
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

            if (!organizationMap.parentId.ToString().IsNullOrEmpty())
            {
                var motherOrgas = _organizationRepository.GetAllOrganizationMothers(organizationMap.Id, new QueryObject(), new OrganizationSearchObject()).Select(o => o.Id).ToList();

                if(motherOrgas != null)
                {
                    var motherTemplates = _templateRepository.GetTemplatesOwnedByOrganizations(motherOrgas, new QueryObject(), new TemplateSearchObject()).Select(t => t.Id).ToList();

                    if (motherTemplates.Any())
                    {
                        foreach (var motherTemplate in motherTemplates)
                        {
                            if (!_templateOrganizationRepository.TemplateOrganizationExists(motherTemplate, organizationMap.Id))
                            {
                                _templateOrganizationRepository.AddTemplateToOrganization(organizationMap.Id, motherTemplate);
                            }
                        }
                    }
                }
            }

            var organization = _mapper.Map<OrganizationDto>(organizationMap);

            return Ok(organization);
        }

        // POST: api/organizations/{id}/user/{userId}/role
        [HttpPost("{id}/user/{userId}/role/{roleId}")]
        [Authorize(Roles = "Admin,Leiter")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUserOrganizationRole(long userId, long id, long roleId)
        {
            var userRole = _roleRepository.GetRole(roleId);
            
            var orgaIds = User.GetOrganizationIds();
            var roles = User.GetRoles();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(id) || userRole.Name == "Admin")
                {
                    return Unauthorized();
                }
            }
            
            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();
                
            if(!_userRepository.UserExists(userId))
                return NotFound();
                
            if(!_roleRepository.RoleExists(roleId))
                return NotFound();
                
            if(_userRepository.UserOrganizationExists(userId, id) == true)
            {
                ModelState.AddModelError("", "User already has a Role in this Organization.");
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

            return Ok(uor);
        }

        // DELETE: api/organization/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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

            if(templateOrganizationsToDelete != null)
            {
                if (!_templateOrganizationRepository.DeleteTemplateOrganizationEntries(templateOrganizationsToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong when deleting template organization.");
                }
            }

            if (!_organizationRepository.DeleteOrganization(organizationToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting Organization.");
            }

            if(userOrganizationRolesToDelete != null)
            {
                if (!_userRepository.DeleteUserOrganizationRoles(userOrganizationRolesToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong deleting Organization Roles.");
                }
            }

            return NoContent();
        }

        // DELETE: api/organization/{id}/user/{userId}/role/{roleId}
        [HttpDelete("{id}/user/{userId}")]
        [Authorize(Roles = "Admin,Leiter")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(long id, long userId)
        {            
            if (!_userRepository.UserOrganizationExists(userId, id))
            {
                return NotFound();
            }

            var userOrganizationRole = _userRepository.GetUserOrganizationRoleEntriesByUser(id).Where(uor => uor.organizationId == id).ToList();

            var userRole = _roleRepository.GetRole(userOrganizationRole.Select(u => u.roleId).Max());

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(id) || userRole.Name == "Admin")
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Leiter")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOrganization(long id, [FromBody] OrganizationDto organizationUpdate)
        {            
            var orgaIds = User.GetOrganizationIds();
            var roles = User.GetRoles();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(id))
                {
                    return Unauthorized();
                }
            }

            if(!_organizationRepository.OrganizationExists(id))
                return NotFound();
                
            if (organizationUpdate == null)
                return BadRequest(ModelState);

            if (id != organizationUpdate.Id)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            var organizationMap = _mapper.Map<Organization>(organizationUpdate);

            if (!_organizationRepository.UpdateOrganization(organizationMap))
            {
                ModelState.AddModelError("", "Something went wrong updating organization.");
                return StatusCode(500, ModelState);
            }

            if (!organizationMap.parentId.ToString().IsNullOrEmpty())
            {
                var motherOrgas = _organizationRepository.GetAllOrganizationMothers(organizationMap.Id, new QueryObject(), new OrganizationSearchObject()).Select(o => o.Id).ToList();

                if(motherOrgas != null)
                {
                    var motherTemplates = _templateRepository.GetTemplatesOwnedByOrganizations(motherOrgas, new QueryObject(), new TemplateSearchObject()).Select(t => t.Id).ToList();

                    if (motherTemplates.Any())
                    {
                        foreach (var motherTemplate in motherTemplates)
                        {
                            if (!_templateOrganizationRepository.TemplateOrganizationExists(motherTemplate, organizationMap.Id))
                            {
                                _templateOrganizationRepository.AddTemplateToOrganization(organizationMap.Id, motherTemplate);
                            }
                        }
                    }
                }
            }

            return Ok(organizationMap);
        }
    }
}