using Models;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using AutoMapper;
using Dto;
using Microsoft.Extensions.Configuration.UserSecrets;
using Helper;
using Helper.SearchObjects;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class ProtocolController : ControllerBase
    {
        private readonly IProtocolRepository _protocolRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IProtocolContentRepository _protocolContentRepository;
        private readonly IProtocolPdfFileRepository _protocolPdfFileRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAdditionalUserRepository _additionalUserRepository;
        private readonly IMapper _mapper;

        public ProtocolController(IProtocolRepository protocolRepository, IOrganizationRepository organizationRepository, IProtocolContentRepository protocolContentRepository, IProtocolPdfFileRepository protocolPdfFileRepository, ITemplateRepository templateRepository, IUserRepository userRepository, IAdditionalUserRepository additionalUserRepository, IMapper mapper)
        {
            _protocolRepository = protocolRepository;
            _organizationRepository = organizationRepository;
            _protocolContentRepository = protocolContentRepository;
            _protocolPdfFileRepository = protocolPdfFileRepository;
            _templateRepository = templateRepository;
            _userRepository = userRepository;
            _additionalUserRepository = additionalUserRepository;
            _mapper = mapper;
        }

        // GET: api/protocols
        [HttpGet]
        [Route("/api/protocols")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Protocol>))]
        public IActionResult GetProtocols([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] ProtocolSearchObject protocolSearchQuery = null)
        {
            var query = _protocolRepository.GetProtocols(dateQuery, protocolSearchQuery).AsQueryable();

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

        // GET: api/protocol/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Protocol))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocol(long id)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var protocol = _mapper.Map<ProtocolDto>(_protocolRepository.GetProtocol(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(protocol);
        }

        // GET: api/protocol/{id}/organization
        [HttpGet("{id}/organization")]
        [ProducesResponseType(200, Type = typeof(Organization))]
        [ProducesResponseType(400)]
        public IActionResult GetOrganizationByProtocol(long id)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var organization = _mapper.Map<OrganizationDto>(_organizationRepository.GetOrganizationByProtocol(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(organization);
        }

        // GET: api/protocol/{id}/content
        [HttpGet("{id}/content")]
        [ProducesResponseType(200, Type = typeof(ProtocolContent))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocolContent(long id)
        {            
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var content = _mapper.Map<ProtocolContentDto>(_protocolContentRepository.GetProtocolContent(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(content);
        }

        // GET: api/protocol/{id}/pdf
        [HttpGet("{id}/pdf")]
        [ProducesResponseType(200, Type = typeof(ProtocolPdfFile))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocolPdfFile(long id)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var pdf = _mapper.Map<ProtocolPdfFileDto>(_protocolPdfFileRepository.GetProtocolPdfFile(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pdf);
        }

        // GET: api/protocol/{id}/template
        [HttpGet("{id}/template")]
        [ProducesResponseType(200, Type = typeof(Template))]
        [ProducesResponseType(400)]
        public IActionResult GetTemplateByProtocol(long id)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var template = _mapper.Map<TemplateDto>(_templateRepository.GetTemplateByProtocol(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(template);
        }

        // GET: api/protocol/{id}/user
        [HttpGet("{id}/user")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUserByProtocol(long id)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userRepository.GetUserByProtocol(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        // GET: api/protocol/{id}/additional-users
        [HttpGet("{id}/additional-users")]
        [ProducesResponseType(200, Type = typeof(ICollection<User>))]
        [ProducesResponseType(400)]
        public IActionResult GetAdditionalUsersByProtocol(long id, [FromQuery] QueryObject dateQuery = null, [FromQuery] UserSearchObject userSearchQuery = null)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var users = _mapper.Map<List<UserDto>>(_userRepository.GetAdditionalUsersByProtocol(id, dateQuery, userSearchQuery));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        // POST: api/protocol/{id}/content
        [HttpPost("{id}/content")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProtocolContent(long id, [FromBody] ProtocolContentDto protocolContentCreate)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            if (protocolContentCreate == null)
                return BadRequest(ModelState);

            var protocolContent = _protocolContentRepository.GetProtocolContent(id);

            if (protocolContent != null)
            {
                ModelState.AddModelError("", "Content for selected Protocol already exists.");
                return StatusCode(422, ModelState);
            }

            if (!JsonValidationService.IsValidJson(protocolContentCreate.Content))
            {
                ModelState.AddModelError("", "JSON Content is not formatted properly.");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var protocolContentMap = _mapper.Map<ProtocolContent>(protocolContentCreate);

            protocolContentMap.Protocol = _protocolRepository.GetProtocol(id);

            if (!_protocolContentRepository.CreateProtocolContent(protocolContentMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created.");
        }

        // POST: api/protocol/{id}/pdf
        [HttpPost("{id}/pdf")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProtocolPdfFile(long id, [FromBody] ProtocolPdfFileDto protocolPdfFileCreate)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();
                
            if (protocolPdfFileCreate == null)
                return BadRequest(ModelState);

            var protocolPdfFile = _protocolPdfFileRepository.GetProtocolPdfFile(id);

            if (protocolPdfFile != null)
            {
                ModelState.AddModelError("", "Pdf File for selected Protocol already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var protocolPdfFileMap = _mapper.Map<ProtocolPdfFile>(protocolPdfFileCreate);

            protocolPdfFileMap.Protocol = _protocolRepository.GetProtocol(id);

            if (!_protocolPdfFileRepository.CreateProtocolPdfFile(protocolPdfFileMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created.");
        }

        // POST: api/protocols
        [HttpPost]
        [Route("/api/protocols")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProtocol([FromQuery] long templateId, [FromQuery] long organizationId, [FromQuery] long userId, [FromQuery] List<long> additionalUserIds, [FromBody] ProtocolDto protocolCreate)
        {
            if (protocolCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var protocolMap = _mapper.Map<Protocol>(protocolCreate);

            protocolMap.Template = _templateRepository.GetTemplate(templateId);
            protocolMap.Organization = _organizationRepository.GetOrganization(organizationId);
            protocolMap.User = _userRepository.GetUser(userId);

            if (!_protocolRepository.CreateProtocol(additionalUserIds, protocolMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created.");
        }

        // DELETE: api/protocol/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProtocol(long id)
        {
            if (!_protocolRepository.ProtocolExists(id))
            {
                return NotFound();
            }

            var protocolContentToDelete = _protocolContentRepository.GetProtocolContent(id);
            var protocolPdfFileToDelete = _protocolPdfFileRepository.GetProtocolPdfFile(id);
            var protocolRolesToDelete = _protocolRepository.GetProtocol(id);
            var additionalUsersToDelete = _additionalUserRepository.GetAdditionalUserEntriesByProtocol(id);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_protocolContentRepository.DeleteProtocolContent(protocolContentToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting Protocol Content.");
            }

            if (!_protocolPdfFileRepository.DeleteProtocolPdfFile(protocolPdfFileToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting Protocol Pdf File.");
            }

            if (!_additionalUserRepository.DeleteAdditionalUserEntries(additionalUsersToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting User as Additional User.");
            }

            if (!_protocolRepository.DeleteProtocol(protocolRolesToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting Protocol.");
            }

            return NoContent();
        }

        // DELETE: api/protocol/{id}/additional-users/{additionaluserId}
        [HttpDelete]
        [Route("/api/protocol/{id}/additional-users/{additionaluserId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult RemoveAdditionalUserFromProtocol(long id, long additionalUserId)
        {
            if(_additionalUserRepository.AdditionalUserExists(additionalUserId, id) == false)
            {
                return NotFound();
            }

            var additionalUserToDelete = _additionalUserRepository.GetAdditionalUser(additionalUserId, id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_additionalUserRepository.DeleteAdditionalUserEntry(additionalUserToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when removing additional user from protocol.");
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProtocol(long id, [FromQuery] List<long> additionalUserIds, [FromBody] ProtocolDto protocolUpdate)
        {
            if (protocolUpdate == null)
                return BadRequest(ModelState);

            if (!_protocolRepository.ProtocolExists(id))
                return NotFound();

            if (id != protocolUpdate.Id)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            var protocolMap = _mapper.Map<Protocol>(protocolUpdate);

            var additionalUsersToDelete =  _additionalUserRepository.GetAdditionalUserEntriesByProtocol(id);
            
            if (!_additionalUserRepository.DeleteAdditionalUserEntries(additionalUsersToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong updating additional users.");
                return StatusCode(500, ModelState);
            }

            if (!_protocolRepository.UpdateProtocol(additionalUserIds, protocolMap))
            {
                ModelState.AddModelError("", "Something went wrong updating protocol.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{id}/content")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProtocolContent(long id, [FromBody] ProtocolContentDto protocolContentUpdate)
        {
            if (protocolContentUpdate == null)
                return BadRequest(ModelState);

            if (id != protocolContentUpdate.protocolId)
                return BadRequest(ModelState);

            if (!_protocolContentRepository.ProtocolContentExists(id))
                return NotFound();

            if (!JsonValidationService.IsValidJson(protocolContentUpdate.Content))
            {
                ModelState.AddModelError("", "JSON Content is not formatted properly.");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var protocolContentMap = _mapper.Map<ProtocolContent>(protocolContentUpdate);

            if (!_protocolContentRepository.UpdateProtocolContent(protocolContentMap))
            {
                ModelState.AddModelError("", "Something went wrong updating protocol content.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{id}/pdf")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProtocolPdfFile(long id, [FromBody] ProtocolPdfFileDto protocolPdfFileUpdate)
        {
            if (protocolPdfFileUpdate == null)
                return BadRequest(ModelState);

            if (id != protocolPdfFileUpdate.protocolId)
                return BadRequest(ModelState);

            if (!_protocolPdfFileRepository.ProtocolPdfFileExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var protocolPdfFileMap = _mapper.Map<ProtocolPdfFile>(protocolPdfFileUpdate);

            if (!_protocolPdfFileRepository.UpdateProtocolPdfFile(protocolPdfFileMap))
            {
                ModelState.AddModelError("", "Something went wrong updating protocol pdf file.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}