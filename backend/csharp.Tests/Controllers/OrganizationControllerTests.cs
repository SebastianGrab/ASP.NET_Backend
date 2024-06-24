using System.Security.Claims;
using Moq;
using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using backend.Controllers;
using Interfaces;
using Dto;
using Models;
using Helper.SearchObjects;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Helper;
using Helper.SeachObjects;

public class OrganizationControllerTests
{
    private readonly Mock<IOrganizationRepository> _mockOrganizationRepository;
    private readonly Mock<IRoleRepository> _mockRoleRepository;
    private readonly Mock<IProtocolRepository> _mockProtocolRepository;
    private readonly Mock<ITemplateRepository> _mockTemplateRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ITemplateOrganizationRepository> _mockTemplateOrganizationRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly OrganizationController _controller;

    public OrganizationControllerTests()
    {
        _mockOrganizationRepository = new Mock<IOrganizationRepository>();
        _mockRoleRepository = new Mock<IRoleRepository>();
        _mockProtocolRepository = new Mock<IProtocolRepository>();
        _mockTemplateRepository = new Mock<ITemplateRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockTemplateOrganizationRepository = new Mock<ITemplateOrganizationRepository>();
        _mockMapper = new Mock<IMapper>();

        _controller = new OrganizationController(
            _mockOrganizationRepository.Object,
            _mockProtocolRepository.Object,
            _mockRoleRepository.Object,
            _mockTemplateRepository.Object,
            _mockUserRepository.Object,
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
    public void GetOrganizations_ReturnsOkResult_WithListOfOrganizations()
    {
        // Arrange
        var organizations = new List<Organization> 
        { 
            new Organization 
            { 
                Id = 1, 
                Name = "Organization1", 
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
            } 
        };
        var organizationDtos = new List<OrganizationDto> 
        { 
            new OrganizationDto 
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

        _mockOrganizationRepository.Setup(repo => repo.GetOrganizations(It.IsAny<QueryObject>(), It.IsAny<OrganizationSearchObject>()))
            .Returns(organizations);
        _mockMapper.Setup(m => m.Map<List<OrganizationDto>>(It.IsAny<List<Organization>>()))
            .Returns(organizationDtos);

        // Act
        var result = _controller.GetOrganizations();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var json = JsonConvert.SerializeObject(okResult.Value);
        var response = JsonConvert.DeserializeObject<PaginatedResponse<OrganizationDto>>(json);

        Assert.NotNull(response);
        Assert.Equal(1, response.items.Count());
        Assert.Equal(1, response.items.First().Id);
        Assert.Equal("Organization1", response.items.First().Name);
    }

    [Fact]
    public void GetOrganization_ReturnsOkResult_WithOrganization()
    {
        // Arrange
        var organizationId = 1;
        var organization = new Organization 
        { 
            Id = organizationId, 
            Name = "Organization1", 
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
        var organizationDto = new OrganizationDto 
        { 
            Id = organizationId, 
            Name = "Organization1", 
            OrganizationType = "Type1", 
            Address = "123 Street", 
            PostalCode = "12345", 
            City = "City1", 
            CreatedDate = DateTime.UtcNow, 
            UpdatedDate = DateTime.UtcNow 
        };

        _mockOrganizationRepository.Setup(repo => repo.OrganizationExists(organizationId)).Returns(true);
        _mockOrganizationRepository.Setup(repo => repo.GetOrganization(organizationId)).Returns(organization);
        _mockMapper.Setup(m => m.Map<OrganizationDto>(It.IsAny<Organization>())).Returns(organizationDto);

        // Act
        var result = _controller.GetOrganization(organizationId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<OrganizationDto>(okResult.Value);
        Assert.Equal(organizationId, returnValue.Id);
    }

    [Fact]
    public void GetOrganization_ReturnsNotFound_WhenOrganizationDoesNotExist()
    {
        // Arrange
        var organizationId = 1;

        _mockOrganizationRepository.Setup(repo => repo.OrganizationExists(organizationId)).Returns(false);

        // Act
        var result = _controller.GetOrganization(organizationId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void CreateOrganization_ReturnsOkResult_WithCreatedOrganization()
    {
        // Arrange
        var organizationCreate = new OrganizationDto 
        { 
            Id = 0, 
            Name = "New Organization", 
            OrganizationType = "Type1", 
            Address = "123 Street", 
            PostalCode = "12345", 
            City = "City1", 
            CreatedDate = DateTime.UtcNow, 
            UpdatedDate = DateTime.UtcNow 
        };
        var organization = new Organization 
        { 
            Id = 0, 
            Name = "New Organization", 
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
        var createdOrganizationDto = new OrganizationDto 
        { 
            Id = 1, 
            Name = "New Organization", 
            OrganizationType = "Type1", 
            Address = "123 Street", 
            PostalCode = "12345", 
            City = "City1", 
            CreatedDate = DateTime.UtcNow, 
            UpdatedDate = DateTime.UtcNow 
        };

        _mockOrganizationRepository.Setup(repo => repo.GetOrganizations(It.IsAny<QueryObject>(), It.IsAny<OrganizationSearchObject>()))
            .Returns(new List<Organization>());
        _mockOrganizationRepository.Setup(repo => repo.CreateOrganization(It.IsAny<Organization>())).Returns(true);
        _mockMapper.Setup(m => m.Map<Organization>(It.IsAny<OrganizationDto>())).Returns(organization);
        _mockMapper.Setup(m => m.Map<OrganizationDto>(It.IsAny<Organization>())).Returns(createdOrganizationDto);

        // Act
        var result = _controller.CreateOrganization(organizationCreate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<OrganizationDto>(okResult.Value);
        Assert.Equal(createdOrganizationDto.Id, returnValue.Id);
        Assert.Equal(createdOrganizationDto.Name, returnValue.Name);
        Assert.Equal(createdOrganizationDto.OrganizationType, returnValue.OrganizationType);
        Assert.Equal(createdOrganizationDto.Address, returnValue.Address);
        Assert.Equal(createdOrganizationDto.PostalCode, returnValue.PostalCode);
        Assert.Equal(createdOrganizationDto.City, returnValue.City);
        Assert.Equal(createdOrganizationDto.CreatedDate, returnValue.CreatedDate);
        Assert.Equal(createdOrganizationDto.UpdatedDate, returnValue.UpdatedDate);
    }

    [Fact]
    public void DeleteOrganization_ReturnsNotFound_WhenOrganizationDoesNotExist()
    {
        // Arrange
        var organizationId = 1;

        _mockOrganizationRepository.Setup(repo => repo.OrganizationExists(organizationId)).Returns(false);

        // Act
        var result = _controller.DeleteOrganization(organizationId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void UpdateOrganization_ReturnsOkResult_WithUpdatedOrganization()
    {
        // Arrange
        var organizationId = 1;
        var organizationUpdate = new OrganizationDto 
        { 
            Id = organizationId, 
            Name = "Updated Organization", 
            OrganizationType = "Type1", 
            Address = "123 Street", 
            PostalCode = "12345", 
            City = "City1", 
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

        _mockOrganizationRepository.Setup(repo => repo.OrganizationExists(organizationId)).Returns(true);
        _mockOrganizationRepository.Setup(repo => repo.UpdateOrganization(It.IsAny<Organization>())).Returns(true);
        _mockMapper.Setup(m => m.Map<Organization>(It.IsAny<OrganizationDto>())).Returns(organization);

        // Act
        var result = _controller.UpdateOrganization(organizationId, organizationUpdate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Organization>(okResult.Value);
        Assert.Equal(organizationId, returnValue.Id);
        Assert.Equal(organizationUpdate.Name, returnValue.Name);
        Assert.Equal(organizationUpdate.OrganizationType, returnValue.OrganizationType);
        Assert.Equal(organizationUpdate.Address, returnValue.Address);
        Assert.Equal(organizationUpdate.PostalCode, returnValue.PostalCode);
        Assert.Equal(organizationUpdate.City, returnValue.City);
    }

    [Fact]
    public void UpdateOrganization_ReturnsNotFound_WhenOrganizationDoesNotExist()
    {
        // Arrange
        var organizationId = 1;
        var organizationUpdate = new OrganizationDto 
        { 
            Id = organizationId, 
            Name = "Updated Organization", 
            OrganizationType = "Type1", 
            Address = "123 Street", 
            PostalCode = "12345", 
            City = "City1", 
            CreatedDate = DateTime.UtcNow, 
            UpdatedDate = DateTime.UtcNow 
        };

        _mockOrganizationRepository.Setup(repo => repo.OrganizationExists(organizationId)).Returns(false);

        // Act
        var result = _controller.UpdateOrganization(organizationId, organizationUpdate);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
