using Models;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using AutoMapper;
using Dto;
using Microsoft.Extensions.Configuration.UserSecrets;
using Helper;
using Helper.SearchObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Repository;
using csharp.Interfaces;

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
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly ITemplateVersionRepository _templateVersionRepository;

        public ProtocolController(ITemplateVersionRepository templateVersionRepository, IProtocolRepository protocolRepository, IOrganizationRepository organizationRepository, IProtocolContentRepository protocolContentRepository, IProtocolPdfFileRepository protocolPdfFileRepository, ITemplateRepository templateRepository, IUserRepository userRepository, IAdditionalUserRepository additionalUserRepository, IEmailService emailService, IMapper mapper)
        {
            _protocolRepository = protocolRepository;
            _organizationRepository = organizationRepository;
            _protocolContentRepository = protocolContentRepository;
            _protocolPdfFileRepository = protocolPdfFileRepository;
            _templateRepository = templateRepository;
            _userRepository = userRepository;
            _additionalUserRepository = additionalUserRepository;
            _emailService = emailService;
            _mapper = mapper;
            _templateVersionRepository = templateVersionRepository;
        }

        // GET: api/protocols
        [HttpGet]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [Route("/api/protocols")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Protocol>))]
        public IActionResult GetProtocols([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] QueryObject dateQuery = null, [FromQuery] ProtocolSearchObject protocolSearchQuery = null)
        {
            var query = _protocolRepository.GetProtocols(dateQuery, protocolSearchQuery, User).AsQueryable();

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
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(Protocol))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocol(long id)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var protocol = _mapper.Map<ProtocolDto>(_protocolRepository.GetProtocol(id, User));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(protocol);
        }

        // GET: api/protocol/{id}/organization
        [HttpGet("{id}/organization")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(Organization))]
        [ProducesResponseType(400)]
        public IActionResult GetOrganizationByProtocol(long id)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var protocolUserId = _userRepository.GetUserByProtocol(id).Id;
            var protocolAdditionalUserIds = _additionalUserRepository.GetAdditionalUserEntriesByProtocol(id).Select(au => au.userId).ToList().Append(protocolUserId);
            var protocolOrga = _organizationRepository.GetOrganizationByProtocol(id);

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !roles.Contains("Leiter") || !orgaIds.Contains(protocolOrga.Id))
                {
                    if (orgaIds.ToString().IsNullOrEmpty() || !roles.Contains("Helfer") || !protocolAdditionalUserIds.Contains(userClaimId))
                    {
                        return Unauthorized();
                    }
                }
            }

            var organization = _mapper.Map<OrganizationDto>(_organizationRepository.GetOrganizationByProtocol(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(organization);
        }

        // GET: api/protocol/{id}/content
        [HttpGet("{id}/content")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(ProtocolContent))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocolContent(long id)
        {            
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var protocolUserId = _userRepository.GetUserByProtocol(id).Id;
            var protocolAdditionalUserIds = _additionalUserRepository.GetAdditionalUserEntriesByProtocol(id).Select(au => au.userId).ToList().Append(protocolUserId);
            var protocolOrga = _organizationRepository.GetOrganizationByProtocol(id);

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !roles.Contains("Leiter") || !orgaIds.Contains(protocolOrga.Id))
                {
                    if (orgaIds.ToString().IsNullOrEmpty() || !roles.Contains("Helfer") || !protocolAdditionalUserIds.Contains(userClaimId))
                    {
                        return Unauthorized();
                    }
                }
            }

            var content = _mapper.Map<ProtocolContentDto>(_protocolContentRepository.GetProtocolContent(id, User));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(content);
        }

        // GET: api/protocol/{id}/pdf-content
        [HttpGet("{id}/pdf-content")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(ProtocolPdfFile))]
        [ProducesResponseType(400)]
        public IActionResult GetProtocolPdfFile(long id)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var protocolUserId = _userRepository.GetUserByProtocol(id).Id;
            var protocolAdditionalUserIds = _additionalUserRepository.GetAdditionalUserEntriesByProtocol(id).Select(au => au.userId).ToList().Append(protocolUserId);
            var protocolOrga = _organizationRepository.GetOrganizationByProtocol(id);

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !roles.Contains("Leiter") || !orgaIds.Contains(protocolOrga.Id))
                {
                    if (orgaIds.ToString().IsNullOrEmpty() || !roles.Contains("Helfer") || !protocolAdditionalUserIds.Contains(userClaimId))
                    {
                        return Unauthorized();
                    }
                }
            }

            var pdf = _mapper.Map<ProtocolPdfFileDto>(_protocolPdfFileRepository.GetProtocolPdfFile(id, User));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pdf);
        }

        // GET: api/protocol/{id}/pdf-download
        [HttpGet("{id}/pdf-download")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(ProtocolPdfFile))]
        [ProducesResponseType(400)]
        public IActionResult DownloadProtocolPdfFile(long id)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var protocolUserId = _userRepository.GetUserByProtocol(id).Id;
            var protocolAdditionalUserIds = _additionalUserRepository.GetAdditionalUserEntriesByProtocol(id).Select(au => au.userId).ToList().Append(protocolUserId);
            var protocolOrga = _organizationRepository.GetOrganizationByProtocol(id);

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !roles.Contains("Leiter") || !orgaIds.Contains(protocolOrga.Id))
                {
                    if (orgaIds.ToString().IsNullOrEmpty() || !roles.Contains("Helfer") || !protocolAdditionalUserIds.Contains(userClaimId))
                    {
                        return Unauthorized();
                    }
                }
            }

            var pdf = _mapper.Map<ProtocolPdfFileDto>(_protocolPdfFileRepository.GetProtocolPdfFile(id, User));

            if(pdf == null)
                return NotFound();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var number = _protocolContentRepository.GetProtocolNumber(id);

            byte[] fileBytes = Convert.FromBase64String(pdf.Content);

            return File(fileBytes, "application/pdf", "Protocol_" + number + "_" + pdf.CreatedDate.Year.ToString() + ".pdf");
        }

        // GET: api/protocol/{id}/template
        [HttpGet("{id}/template")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(Template))]
        [ProducesResponseType(400)]
        public IActionResult GetTemplateByProtocol(long id)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var protocolUserId = _userRepository.GetUserByProtocol(id).Id;
            var protocolAdditionalUserIds = _additionalUserRepository.GetAdditionalUserEntriesByProtocol(id).Select(au => au.userId).ToList().Append(protocolUserId);
            var protocolOrga = _organizationRepository.GetOrganizationByProtocol(id);

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !roles.Contains("Leiter") || !orgaIds.Contains(protocolOrga.Id))
                {
                    if (orgaIds.ToString().IsNullOrEmpty() || !roles.Contains("Helfer") || !protocolAdditionalUserIds.Contains(userClaimId))
                    {
                        return Unauthorized();
                    }
                }
            }

            var template = _mapper.Map<TemplateDto>(_templateRepository.GetTemplateByProtocol(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(template);
        }

        // GET: api/protocol/{id}/user
        [HttpGet("{id}/user")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUserByProtocol(long id)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var protocolUserId = _userRepository.GetUserByProtocol(id).Id;
            var protocolAdditionalUserIds = _additionalUserRepository.GetAdditionalUserEntriesByProtocol(id).Select(au => au.userId).ToList().Append(protocolUserId);
            var protocolOrga = _organizationRepository.GetOrganizationByProtocol(id);

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !roles.Contains("Leiter") || !orgaIds.Contains(protocolOrga.Id))
                {
                    if (orgaIds.ToString().IsNullOrEmpty() || !roles.Contains("Helfer") || !protocolAdditionalUserIds.Contains(userClaimId))
                    {
                        return Unauthorized();
                    }
                }
            }

            var user = _mapper.Map<UserDto>(_userRepository.GetUserByProtocol(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        // GET: api/protocol/{id}/additional-users
        [HttpGet("{id}/additional-users")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(ICollection<User>))]
        [ProducesResponseType(400)]
        public IActionResult GetAdditionalUsersByProtocol(long id, [FromQuery] QueryObject dateQuery = null, [FromQuery] UserSearchObject userSearchQuery = null)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var protocolUserId = _userRepository.GetUserByProtocol(id).Id;
            var protocolAdditionalUserIds = _additionalUserRepository.GetAdditionalUserEntriesByProtocol(id).Select(au => au.userId).ToList().Append(protocolUserId);
            var protocolOrga = _organizationRepository.GetOrganizationByProtocol(id);

            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !roles.Contains("Leiter") || !orgaIds.Contains(protocolOrga.Id))
                {
                    if (orgaIds.ToString().IsNullOrEmpty() || !roles.Contains("Helfer") || !protocolAdditionalUserIds.Contains(userClaimId))
                    {
                        return Unauthorized();
                    }
                }
            }

            var users = _mapper.Map<List<UserDto>>(_userRepository.GetAdditionalUsersByProtocol(id, dateQuery, userSearchQuery));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        // GET: api/no-of-protocols-to-review
        [HttpGet]
        [Route("/api/no-of-protocols-to-review")]
        [Authorize(Roles = "Admin,Leiter")]
        [ProducesResponseType(200, Type = typeof(int))]
        public IActionResult GetNumberOfProtocolsToReview(long id)
        {
            var numberOfProtocolsToReview = _protocolRepository.GetNumberOfProtocolsToReview(User);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfProtocolsToReview);
        }

        // POST: api/protocol/{id}/content
        [HttpPost("{id}/content")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProtocolContent(long id, [FromBody] ProtocolContentDto protocolContentCreate)
        {
            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();

            var protocolUser = _userRepository.GetUserByProtocol(id);

            var roles = User.GetRoles();
            var userId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (userId.ToString().IsNullOrEmpty() || userId != protocolUser.Id)
                {
                    return Unauthorized();
                }
            }

            if (protocolContentCreate == null)
                return BadRequest(ModelState);

            if (_protocolContentRepository.ProtocolContentExists(id))
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

            protocolContentMap.Protocol = _protocolRepository.GetProtocol(id, User);
            protocolContentMap.protocolId = id;
            protocolContentMap.Id = id;

            var templateId = _templateRepository.GetTemplateByProtocol(id);

            if(templateId == null)
                return NotFound();

            var templateVersions = _templateVersionRepository.GetVersions(templateId.Id).Select(tv => tv.TemplateContent).ToList();

            if (!ProtocolValidationService.IsValidProtocol(templateVersions, protocolContentCreate.Content))
            {
                ModelState.AddModelError("", "JSON Content does not match with a Template.");
                return StatusCode(400, ModelState);
            }

            if (!_protocolContentRepository.CreateProtocolContent(protocolContentMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            var protocolContentToReturn = _mapper.Map<ProtocolContentDto>(protocolContentMap);

            return Ok(protocolContentToReturn);
        }

        // POST: api/protocol/{id}/pdf
        [HttpPost("{id}/pdf")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProtocolPdfFile(long id, [FromBody] ProtocolPdfFileDto protocolPdfFileCreate)
        {
            var protocolUser = _userRepository.GetUserByProtocol(id);

            var roles = User.GetRoles();
            var userId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (userId.ToString().IsNullOrEmpty() || userId != protocolUser.Id)
                {
                    return Unauthorized();
                }
            }

            if(!_protocolRepository.ProtocolExists(id))
                return NotFound();
                
            if (protocolPdfFileCreate == null)
                return BadRequest(ModelState);

            var protocolPdfFile = _protocolPdfFileRepository.GetProtocolPdfFile(id, User);

            if (protocolPdfFile != null)
            {
                ModelState.AddModelError("", "Pdf File for selected Protocol already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var protocolPdfFileMap = _mapper.Map<ProtocolPdfFile>(protocolPdfFileCreate);

            protocolPdfFileMap.Protocol = _protocolRepository.GetProtocol(id, User);
            protocolPdfFileMap.protocolId = id;
            protocolPdfFileMap.Id = id;

            if (!_protocolPdfFileRepository.CreateProtocolPdfFile(protocolPdfFileMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            var protocolPdfFileToReturn = _mapper.Map<ProtocolPdfFileDto>(protocolPdfFileMap);

            return Ok(protocolPdfFileToReturn);
        }

        // POST: api/protocols
        [HttpPost]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [Route("/api/protocols")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProtocol([FromQuery] long templateId, [FromQuery] long organizationId, [FromQuery] long userId, [FromQuery] List<long> additionalUserIds, [FromBody] ProtocolDto protocolCreate)
        {
            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(organizationId) || userClaimId.ToString().IsNullOrEmpty() || userClaimId != userId)
                {
                    return Unauthorized();
                }
            }

            if (protocolCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var protocolMap = _mapper.Map<Protocol>(protocolCreate);
            
            if(!_templateRepository.TemplateExists(templateId))
                return NotFound();
                
            if(!_userRepository.UserExists(userId))
                return NotFound();
                
            if(!_organizationRepository.OrganizationExists(organizationId))
                return NotFound();

            foreach (var additionalUserId in additionalUserIds)
            {
                if(!_userRepository.UserExists(additionalUserId))
                    return NotFound();
            }

            protocolMap.Template = _templateRepository.GetTemplate(templateId);
            protocolMap.Organization = _organizationRepository.GetOrganization(organizationId);
            protocolMap.User = _userRepository.GetUser(userId);

            if (!_protocolRepository.CreateProtocol(additionalUserIds, protocolMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            if (!protocolMap.IsDraft && !protocolMap.IsClosed && protocolMap.sendEmail == true && protocolMap.emailSubject != null && protocolMap.emailContent != null)
            {
                var mailReceivers = _userRepository.GetUsersByOrganizationAndRole(protocolMap.Organization.Id, -2);

                if (mailReceivers != null)
                {
                    List<User> emailUsers = mailReceivers.Where(p => p.Id != userClaimId).ToList();

                    if (emailUsers != null)
                    {
                        _emailService.SendEmailFromProtocol(emailUsers, protocolMap.emailSubject, protocolMap.emailContent, protocolMap.Id);
                    }
                }
            }

            try 
            {
                var protocolToReturn = _mapper.Map<ProtocolDto>(_protocolRepository.GetProtocol(protocolMap.Id, User));

                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(protocolToReturn);
            }
            catch
            {
                return Ok("Created successfully");
            }
        }

        // DELETE: api/protocol/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProtocol(long id)
        {
            if (!_protocolRepository.ProtocolExists(id))
            {
                return NotFound();
            }

            var protocolUser = _userRepository.GetUserByProtocol(id);
            var roles = User.GetRoles();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (protocolUser == null || userClaimId.ToString() == null || userClaimId.ToString().IsNullOrEmpty() || userClaimId != protocolUser.Id)
                {
                    return Unauthorized();
                }
            }

            var protocolContentToDelete = _protocolContentRepository.GetProtocolContent(id, User);
            var protocolPdfFileToDelete = _protocolPdfFileRepository.GetProtocolPdfFile(id, User);
            var protocolsToDelete = _protocolRepository.GetProtocol(id, User);
            var additionalUsersToDelete = _additionalUserRepository.GetAdditionalUserEntriesByProtocol(id);

            if (protocolsToDelete.IsClosed) 
            {
                ModelState.AddModelError("", "Can't delete a closed protocol.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (protocolContentToDelete != null) {
                if (!_protocolContentRepository.DeleteProtocolContent(protocolContentToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong when deleting Protocol Content.");
                }
            }

            if (protocolPdfFileToDelete != null) { 
                if (!_protocolPdfFileRepository.DeleteProtocolPdfFile(protocolPdfFileToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong when deleting Protocol Pdf File.");
                }
            }

            if (additionalUsersToDelete != null) {
                if (!_additionalUserRepository.DeleteAdditionalUserEntries(additionalUsersToDelete.ToList()))
                {
                    ModelState.AddModelError("", "Something went wrong when deleting User as Additional User.");
                }
            }

            if (!_protocolRepository.DeleteProtocol(protocolsToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting Protocol.");
            }

            return NoContent();
        }

        // DELETE: api/protocol/{id}/additional-users/{additionaluserId}
        [HttpDelete]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
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
            
            var protocolUser = _userRepository.GetUserByProtocol(id);
            var roles = User.GetRoles();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (userClaimId.ToString().IsNullOrEmpty() || userClaimId != protocolUser.Id)
                {
                    return Unauthorized();
                }
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
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProtocol(long id, [FromQuery] List<long> additionalUserIds, [FromBody] ProtocolDto protocolUpdate)
        {
            var protocolOrga = _organizationRepository.GetOrganizationByProtocol(id);
            var roles = User.GetRoles();
            var orgaIds = User.GetOrganizationIds();
            var closingUserId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (protocolOrga.Id.ToString().IsNullOrEmpty() || orgaIds.ToString().IsNullOrEmpty() || !orgaIds.Contains(protocolOrga.Id))
                {
                    return Unauthorized();
                }
            }
            if (protocolUpdate == null)
                return BadRequest(ModelState);

            if (!_protocolRepository.ProtocolExists(id))
                return NotFound();

            if (!protocolUpdate.IsClosed && closingUserId.ToString() == null)
            {
                ModelState.AddModelError("", "Closing user Id required.");
                return StatusCode(500, ModelState);
            }
            
            if (additionalUserIds.Count() >= 1)
            {
                foreach (var additionalUserId in additionalUserIds)
                {
                    if (!additionalUserId.ToString().IsNullOrEmpty())
                    {
                        if(!_userRepository.UserExists(additionalUserId))
                            return NotFound();
                    }
                }
            }

            if (id != protocolUpdate.Id)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            var protocolMap = _mapper.Map<Protocol>(protocolUpdate);

            var protocolOrganisation = _organizationRepository.GetOrganizationByProtocol(id);
            var protocolUser = _userRepository.GetUserByProtocol(id);
            var protocolTemplate = _templateRepository.GetTemplateByProtocol(id);

            protocolMap.Organization = protocolOrganisation;
            protocolMap.User = protocolUser;
            protocolMap.Template = protocolTemplate;

            var additionalUsersToDelete =  _additionalUserRepository.GetAdditionalUserEntriesByProtocol(id);
            
            if (additionalUsersToDelete.Count() != 0)
            {
                if (!_additionalUserRepository.DeleteAdditionalUserEntries(additionalUsersToDelete.ToList()))
                {
                    ModelState.AddModelError("", "Something went wrong updating additional users.");
                    return StatusCode(500, ModelState);
                }
            }

            if (!_protocolRepository.UpdateProtocol(closingUserId, additionalUserIds, protocolMap))
            {
                ModelState.AddModelError("", "Something went wrong updating protocol.");
                return StatusCode(500, ModelState);
            }

            if (!protocolMap.IsDraft && !protocolMap.IsClosed && protocolMap.sendEmail == true && protocolMap.emailSubject != null && protocolMap.emailContent != null)
            {
                var mailReceivers = _userRepository.GetUsersByOrganizationAndRole(protocolMap.Organization.Id, -2);

                if (mailReceivers != null)
                {
                    List<User> emailUsers = mailReceivers.Where(p => p.Id != closingUserId).ToList();

                    if (emailUsers != null)
                    {
                        _emailService.SendEmailFromProtocol(emailUsers, protocolMap.emailSubject, protocolMap.emailContent, id);
                    }
                }                
            }


            return Ok(protocolMap);
        }

        [HttpPut("{id}/content")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProtocolContent(long id, [FromBody] ProtocolContentDto protocolContentUpdate)
        {
            if (!_protocolRepository.ProtocolExists(id))
                return NotFound();

            if (!_protocolContentRepository.ProtocolContentExists(id))
                return NotFound();

            var protocolUser = _userRepository.GetUserByProtocol(id);
            var roles = User.GetRoles();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (userClaimId.ToString().IsNullOrEmpty() || userClaimId != protocolUser.Id)
                {
                    return Unauthorized();
                }
            }

            if (protocolContentUpdate == null)
                return BadRequest(ModelState);

            if (id != protocolContentUpdate.protocolId)
                return BadRequest(ModelState);

            if (!JsonValidationService.IsValidJson(protocolContentUpdate.Content))
            {
                ModelState.AddModelError("", "JSON Content is not formatted properly.");
                return StatusCode(400, ModelState);
            }

            var protocol = _protocolRepository.GetProtocol(id, User);

            if (protocol == null || protocol.IsClosed)
            {
                ModelState.AddModelError("", "Cannot update a closed protocol.");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var protocolContentMap = _mapper.Map<ProtocolContent>(protocolContentUpdate);
            protocolContentMap.Id = id;

            var templateId = _templateRepository.GetTemplateByProtocol(id);

            if(templateId == null)
                return NotFound();

            var templateVersions = _templateVersionRepository.GetVersions(templateId.Id).Select(tv => tv.TemplateContent).ToList();

            if (!ProtocolValidationService.IsValidProtocol(templateVersions, protocolContentUpdate.Content))
            {
                ModelState.AddModelError("", "JSON Content does not match with a Template.");
                return StatusCode(400, ModelState);
            }

            if (!_protocolContentRepository.UpdateProtocolContent(protocolContentMap))
            {
                ModelState.AddModelError("", "Something went wrong updating protocol content.");
                return StatusCode(500, ModelState);
            }

            return Ok(protocolContentMap);
        }

        [HttpPut("{id}/pdf")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProtocolPdfFile(long id, [FromBody] ProtocolPdfFileDto protocolPdfFileUpdate)
        {
            if (!_protocolRepository.ProtocolExists(id))
                return NotFound();

            if (!_protocolContentRepository.ProtocolContentExists(id))
                return NotFound();

            var protocolUser = _userRepository.GetUserByProtocol(id);
            var roles = User.GetRoles();
            var userClaimId = User.GetUserId();

            if (roles.IsNullOrEmpty() || !roles.Contains("Admin"))
            {
                if (userClaimId.ToString().IsNullOrEmpty() || userClaimId != protocolUser.Id)
                {
                    return Unauthorized();
                }
            }
            
            if (protocolPdfFileUpdate == null)
                return BadRequest(ModelState);

            if (id != protocolPdfFileUpdate.protocolId)
                return BadRequest(ModelState);

            if (!_protocolPdfFileRepository.ProtocolPdfFileExists(id))
                return NotFound();

            var protocol = _protocolRepository.GetProtocol(id, User);

            if (protocol == null || protocol.IsClosed)
            {
                ModelState.AddModelError("", "Cannot update a closed protocol.");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var protocolPdfFileMap = _mapper.Map<ProtocolPdfFile>(protocolPdfFileUpdate);
            protocolPdfFileMap.Id = id;

            if (!_protocolPdfFileRepository.UpdateProtocolPdfFile(protocolPdfFileMap))
            {
                ModelState.AddModelError("", "Something went wrong updating protocol pdf file.");
                return StatusCode(500, ModelState);
            }

            return Ok(protocolPdfFileMap);
        }
    }
}