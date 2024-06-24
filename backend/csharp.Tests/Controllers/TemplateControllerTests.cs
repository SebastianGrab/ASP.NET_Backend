using System.Security.Claims; // Namespace hinzufügen
using Moq;
using Xunit;
using AutoMapper;
using backend.Controllers;
using Interfaces;
using Dto;
using Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Helper.SearchObjects;
using Helper;
using Helper.SeachObjects;
public class TemplateControllerTests
{
    private readonly Mock<ITemplateRepository> _mockTemplateRepository;
    private readonly Mock<IOrganizationRepository> _mockOrganizationRepository;
    private readonly Mock<IProtocolRepository> _mockProtocolRepository;
    private readonly Mock<ITemplateOrganizationRepository> _mockTemplateOrganizationRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly TemplateController _controller;

    public TemplateControllerTests()
    {
        _mockTemplateRepository = new Mock<ITemplateRepository>();
        _mockOrganizationRepository = new Mock<IOrganizationRepository>();
        _mockProtocolRepository = new Mock<IProtocolRepository>();
        _mockTemplateOrganizationRepository = new Mock<ITemplateOrganizationRepository>();
        _mockMapper = new Mock<IMapper>();

        _controller = new TemplateController(
            _mockTemplateRepository.Object,
            _mockOrganizationRepository.Object,
            _mockProtocolRepository.Object,
            _mockTemplateOrganizationRepository.Object,
            _mockMapper.Object);

        // Setup the ClaimsPrincipal
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim("Organization", "1"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim("UserId", "1")
        };
        var identity = new ClaimsIdentity(claims, "mock");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            }
        };
    }

    [Fact]
    public void GetTemplates_ReturnsOkResult_WithListOfTemplates()
    {
        // Arrange
        var templates = new List<Template> 
        { 
            new Template 
            { 
                Id = 1, 
                Name = "Template1", 
                TemplateContent = "{}", 
                CreatedDate = DateTime.UtcNow, 
                UpdatedDate = DateTime.UtcNow,
                Organization = new Organization 
                { 
                    Id = 1, 
                    Name = "Organization1", 
                    OrganizationType = "Type1", 
                    Address = "123 Street", 
                    PostalCode = "12345", 
                    City = "City1", 
                    CreatedDate = DateTime.UtcNow, 
                    UpdatedDate = DateTime.UtcNow 
                }
            } 
        };
        var templateDtos = new List<TemplateDto> 
        { 
            new TemplateDto 
            { 
                Id = 1, 
                Name = "Template1", 
                TemplateContent = "{}", 
                CreatedDate = DateTime.UtcNow, 
                UpdatedDate = DateTime.UtcNow 
            } 
        };

        _mockTemplateRepository.Setup(repo => repo.GetTemplates(It.IsAny<QueryObject>(), It.IsAny<TemplateSearchObject>()))
            .Returns(templates);
        _mockMapper.Setup(m => m.Map<List<TemplateDto>>(It.IsAny<List<Template>>()))
            .Returns(templateDtos);

        // Act
        var result = _controller.GetTemplates();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var json = JsonConvert.SerializeObject(okResult.Value);
        var response = JsonConvert.DeserializeObject<PaginatedResponse<TemplateDto>>(json);

        Assert.NotNull(response);
        Assert.Equal(1, response.items.Count());
        Assert.Equal(1, response.items.First().Id);
        Assert.Equal("Template1", response.items.First().Name);
    }

    [Fact]
    public void GetTemplate_ReturnsOkResult_WithTemplate()
    {
        // Arrange
        var templateId = 1;
        var template = new Template 
        { 
            Id = templateId, 
            Name = "Template1", 
            TemplateContent = "{}", 
            CreatedDate = DateTime.UtcNow, 
            UpdatedDate = DateTime.UtcNow,
            Organization = new Organization 
            { 
                Id = 1, 
                Name = "Organization1", 
                OrganizationType = "Type1", 
                Address = "123 Street", 
                PostalCode = "12345", 
                City = "City1", 
                CreatedDate = DateTime.UtcNow, 
                UpdatedDate = DateTime.UtcNow 
            }
        };
        var templateDto = new TemplateDto 
        { 
            Id = templateId, 
            Name = "Template1", 
            TemplateContent = "{}", 
            CreatedDate = DateTime.UtcNow, 
            UpdatedDate = DateTime.UtcNow 
        };

        _mockTemplateRepository.Setup(repo => repo.TemplateExists(templateId)).Returns(true);
        _mockTemplateRepository.Setup(repo => repo.GetTemplate(templateId)).Returns(template);
        _mockMapper.Setup(m => m.Map<TemplateDto>(It.IsAny<Template>())).Returns(templateDto);

        // Act
        var result = _controller.GetTemplate(templateId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<TemplateDto>(okResult.Value);
        Assert.Equal(templateId, returnValue.Id);
    }

    [Fact]
    public void CreateTemplate_ReturnsOkResult_WithCreatedTemplate()
    {
        // Arrange
        var organizationId = 1;
        var templateCreate = new TemplateDto 
        { 
            Id = 0, 
            Name = "New Template", 
            Description = "Description", 
            TemplateContent = "{\"Name\":\"Protokollschema\",\"Schema\":[{\"Kategorie\":\"Schlüssel\",\"ID\":\"SCHLUESSEL\",\"Inputs\":[{\"Name\":\"Alarmschluessel\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Alarmschlüssel:\",\"ID\":\"ALARMSCHLUESSEL\",\"Mandatory\":1,\"Pattern\":\"^[123][0-9]{3}[NBnb]?$\",\"Placeholder\":\"1000N\"},{\"Name\":\"Auftragsnummer\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"AuftragsNr:\",\"ID\":\"AUFTRAGSNUMMER\",\"Mandatory\":1,\"DisableHandlerId\":\"Auftragsnummer-Mandatoryhandler\",\"Pattern\":\"^([0-9]+)$\",\"Placeholder\":\"123456\"},{\"Name\":\"Auftragsnummer-Mandatoryhandler\",\"Element\":\"mandatoryhandler\",\"Type\":\"mandatoryhandler\",\"Label\":\"KeineAuftragsnummer:\",\"ID\":\"Auftragsnummer-Mandatoryhandler\"}]}]}", 
            CreatedDate = DateTime.UtcNow, 
            UpdatedDate = DateTime.UtcNow 
        };
        var template = new Template 
        { 
            Id = 0, 
            Name = "New Template", 
            Description = "Description", 
            TemplateContent = "{\"Name\":\"Protokollschema\",\"Schema\":[{\"Kategorie\":\"Schlüssel\",\"ID\":\"SCHLUESSEL\",\"Inputs\":[{\"Name\":\"Alarmschluessel\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Alarmschlüssel:\",\"ID\":\"ALARMSCHLUESSEL\",\"Mandatory\":1,\"Pattern\":\"^[123][0-9]{3}[NBnb]?$\",\"Placeholder\":\"1000N\"},{\"Name\":\"Auftragsnummer\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"AuftragsNr:\",\"ID\":\"AUFTRAGSNUMMER\",\"Mandatory\":1,\"DisableHandlerId\":\"Auftragsnummer-Mandatoryhandler\",\"Pattern\":\"^([0-9]+)$\",\"Placeholder\":\"123456\"},{\"Name\":\"Auftragsnummer-Mandatoryhandler\",\"Element\":\"mandatoryhandler\",\"Type\":\"mandatoryhandler\",\"Label\":\"KeineAuftragsnummer:\",\"ID\":\"Auftragsnummer-Mandatoryhandler\"}]}]}", 
            organizationId = organizationId,
            Organization = new Organization 
            { 
                Id = organizationId, 
                Name = "Organization1", 
                OrganizationType = "Type1", 
                Address = "123 Street", 
                PostalCode = "12345", 
                City = "City1", 
                CreatedDate = DateTime.UtcNow, 
                UpdatedDate = DateTime.UtcNow 
            },
            CreatedDate = DateTime.UtcNow, 
            UpdatedDate = DateTime.UtcNow 
        };
        var createdTemplateDto = new TemplateDto 
        { 
            Id = 1, 
            Name = "New Template", 
            Description = "Description", 
            TemplateContent = "{\"Name\":\"Protokollschema\",\"Schema\":[{\"Kategorie\":\"Schlüssel\",\"ID\":\"SCHLUESSEL\",\"Inputs\":[{\"Name\":\"Alarmschluessel\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Alarmschlüssel:\",\"ID\":\"ALARMSCHLUESSEL\",\"Mandatory\":1,\"Pattern\":\"^[123][0-9]{3}[NBnb]?$\",\"Placeholder\":\"1000N\"},{\"Name\":\"Auftragsnummer\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"AuftragsNr:\",\"ID\":\"AUFTRAGSNUMMER\",\"Mandatory\":1,\"DisableHandlerId\":\"Auftragsnummer-Mandatoryhandler\",\"Pattern\":\"^([0-9]+)$\",\"Placeholder\":\"123456\"},{\"Name\":\"Auftragsnummer-Mandatoryhandler\",\"Element\":\"mandatoryhandler\",\"Type\":\"mandatoryhandler\",\"Label\":\"KeineAuftragsnummer:\",\"ID\":\"Auftragsnummer-Mandatoryhandler\"}]}]}",
            CreatedDate = DateTime.UtcNow, 
            UpdatedDate = DateTime.UtcNow 
        };
        var organization = new Organization 
        { 
            Id = organizationId, 
            Name = "Updated Organization", 
            OrganizationType = "Type1", 
            Address = "123 Street", 
            PostalCode = "12345", 
            City = "City1", 
            CreatedDate = DateTime.UtcNow, 
            UpdatedDate = DateTime.UtcNow,
            TemplateOrganizations = new List<TemplateOrganization>(),
            UserOrganizationRoles = new List<UserOrganizationRole>(),
            Protocols = new List<Protocol>(),
            Templates = new List<Template>()
        };

        _mockTemplateRepository.Setup(repo => repo.GetSharedTemplatesByOrganization(organizationId, It.IsAny<QueryObject>(), It.IsAny<TemplateSearchObject>()))
            .Returns(new List<Template>());
        _mockOrganizationRepository.Setup(repo => repo.OrganizationExists(organizationId)).Returns(true);
        _mockOrganizationRepository.Setup(repo => repo.GetOrganization(organizationId)).Returns(organization);
        _mockTemplateRepository.Setup(repo => repo.CreateTemplate(organizationId, It.IsAny<Template>())).Returns(true);
        _mockMapper.Setup(m => m.Map<Template>(It.IsAny<TemplateDto>())).Returns(template);
        _mockMapper.Setup(m => m.Map<TemplateDto>(It.IsAny<Template>())).Returns(createdTemplateDto);

        // Act
        var result = _controller.CreateTemplate(organizationId, templateCreate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<TemplateDto>(okResult.Value);
        Assert.Equal(createdTemplateDto.Id, returnValue.Id);
        Assert.Equal(createdTemplateDto.Name, returnValue.Name);
        Assert.Equal(createdTemplateDto.Description, returnValue.Description);
        Assert.Equal(createdTemplateDto.TemplateContent, returnValue.TemplateContent);
        Assert.Equal(createdTemplateDto.CreatedDate, returnValue.CreatedDate);
        Assert.Equal(createdTemplateDto.UpdatedDate, returnValue.UpdatedDate);
    }
}
