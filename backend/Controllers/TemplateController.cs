using Models;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using AutoMapper;
using Dto;
using Helper;
using Helper.SearchObjects;
using Helper.SeachObjects;

namespace backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IProtocolRepository _protocolRepository;
        private readonly ITemplateOrganizationRepository _templateOrganizationRepository;
        private readonly IMapper _mapper;

        public TemplateController(ITemplateRepository templateRepository, IOrganizationRepository organizationRepository, IProtocolRepository protocolRepository, ITemplateOrganizationRepository templateOrganizationRepository, IMapper mapper)
        {
            _templateRepository = templateRepository;
            _organizationRepository = organizationRepository;
            _protocolRepository = protocolRepository;
            _templateOrganizationRepository = templateOrganizationRepository;
            _mapper = mapper;
        }

        // GET: api/templates
        [HttpGet]
        [Route("/api/templates")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Template>))]
        public IActionResult GetTemplates([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] TemplateSearchObject templateSearchQuery = null)
        {
            var query = _templateRepository.GetTemplates(dateQuery, templateSearchQuery).AsQueryable();

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

        // GET: api/template/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Template))]
        [ProducesResponseType(400)]
        public IActionResult GetTemplate(long id)
        {
            if(!_templateRepository.TemplateExists(id))
                return NotFound();

            var template = _mapper.Map<TemplateDto>(_templateRepository.GetTemplate(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(template);
        }

        // GET: api/template/{id}/organizations
        [HttpGet("{id}/organizations")]
        [ProducesResponseType(200, Type = typeof(ICollection<Organization>))]
        [ProducesResponseType(400)]
        public IActionResult GetOrganizationsByTemplate(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] OrganizationSearchObject organizationSearchQuery = null)
        {
            if(!_templateRepository.TemplateExists(id))
                return NotFound();

            var query = _organizationRepository.GetOrganizationsByTemplate(id, dateQuery, organizationSearchQuery).AsQueryable();

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

        // GET: api/template/{id}/protocols
        [HttpGet("{id}/protocols")]
        [ProducesResponseType(200, Type = typeof(ICollection<Protocol>))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocolsByTemplate(long id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] ProtocolSearchObject protocolSearchQuery = null)
        {
            if(!_templateRepository.TemplateExists(id))
                return NotFound();

            var query = _protocolRepository.GetProtocolsByTemplate(id, dateQuery, protocolSearchQuery).AsQueryable();

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

        // POST: api/templates
        [HttpPost]
        [Route("/api/templates")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTemplate([FromQuery] long organizationId, [FromBody] TemplateDto templateCreate)
        {
            if (templateCreate == null)
                return BadRequest(ModelState);

            var templateName = _templateRepository.GetSharedTemplatesByOrganization(organizationId, new QueryObject(), new TemplateSearchObject())
                .Where(o => o.Name.Trim().ToUpper() == templateCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (templateName != null)
            {
                ModelState.AddModelError("", "A Template in this Organization with the given Name already exists.");
                return StatusCode(422, ModelState);
            }

            if (!JsonValidationService.IsValidJson(templateCreate.TemplateContent))
            {
                ModelState.AddModelError("", "JSON Content is not formatted properly.");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var templateMap = _mapper.Map<Template>(templateCreate);
            templateMap.Organization = _organizationRepository.GetOrganization(organizationId);

      
            if (!_templateRepository.CreateTemplate(organizationId, templateMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created.");
        }

        // POST: api/template/{id}/add-to-organization/{organizationId}
        [HttpPost]
        [Route("/api/template/{id}/add-to-organization/{organizationId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddTemplateToOrganization(long id, long organizationId)
        {
            if(_templateRepository.TemplateExists(id) == false)
            {
                ModelState.AddModelError("", "Template does not exists.");
                return StatusCode(422, ModelState);
            }
            if(_organizationRepository.OrganizationExists(organizationId) == false)
            {
                ModelState.AddModelError("", "Organization does not exists.");
                return StatusCode(422, ModelState);
            }
            if(_templateOrganizationRepository.TemplateOrganizationExists(id, organizationId) == true)
            {
                ModelState.AddModelError("", "Template is already added to this Organization.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_templateOrganizationRepository.AddTemplateToOrganization(organizationId, id))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully added.");
        }

        // DELETE: api/template/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTemplate(long id)
        {
            if (!_templateRepository.TemplateExists(id))
            {
                return NotFound();
            }

            var templateOrganizationsToDelete = _templateOrganizationRepository.GetTemplateOrganizationEntriesByTemplate(id);
            var templateToDelete = _templateRepository.GetTemplate(id);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_templateOrganizationRepository.DeleteTemplateOrganizationEntries(templateOrganizationsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting template organization.");
            }

            if (!_templateRepository.DeleteTemplate(templateToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting template.");
            }

            return NoContent();
        }

        // DELETE: api/template/{id}/remove-from-organization/{organizationId}
        [HttpDelete]
        [Route("/api/template/{id}/remove-from-organization/{organizationId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult RemoveTemplateFromOrganization(long id, long organizationId)
        {
            if(!_templateOrganizationRepository.TemplateOrganizationExists(id, organizationId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_templateOrganizationRepository.RemoveTemplateFromOrganization(organizationId, id))
            {
                ModelState.AddModelError("", "Something went wrong when removing template from organization.");
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTemplate(long id, [FromBody] TemplateDto templateUpdate)
        {
            if (templateUpdate == null)
                return BadRequest(ModelState);

            if (id != templateUpdate.Id)
                return BadRequest(ModelState);

            if (!_templateRepository.TemplateExists(id))
                return NotFound();

            if (!JsonValidationService.IsValidJson(templateUpdate.TemplateContent))
            {
                ModelState.AddModelError("", "JSON Content is not formatted properly.");
                return StatusCode(400, ModelState);
            }

            if (_organizationRepository.OrganizationExists(_templateRepository.GetTemplate(id).Organization.Id))
            {
                ModelState.AddModelError("", "Owning Organization not found.");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var templateMap = _mapper.Map<Template>(templateUpdate);
            templateMap.Organization = _organizationRepository.GetOrganization(_templateRepository.GetTemplate(id).Organization.Id);

            if (!_templateRepository.UpdateTemplate(templateMap))
            {
                ModelState.AddModelError("", "Something went wrong updating template.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}