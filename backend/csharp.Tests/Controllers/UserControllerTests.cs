using Xunit;
using Moq;
using backend.Controllers;
using Interfaces;
using AutoMapper;
using Dto;
using Microsoft.AspNetCore.Mvc;
using Helper.SearchObjects;
using Models;
using Newtonsoft.Json;
using Helper;
using Helper.SeachObjects;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

public class UserControllerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IOrganizationRepository> _mockOrganizationRepository;
    private readonly Mock<IRoleRepository> _mockRoleRepository;
    private readonly Mock<IProtocolRepository> _mockProtocolRepository;
    private readonly Mock<ITemplateRepository> _mockTemplateRepository;
    private readonly Mock<IUserMessageRepository> _mockUserMessageRepository;
    private readonly Mock<IAdditionalUserRepository> _mockAdditionalUserRepository;
    private readonly Mock<IUserLoginAttemptRepository> _mockUserLoginAttemptRepository;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockOrganizationRepository = new Mock<IOrganizationRepository>();
        _mockRoleRepository = new Mock<IRoleRepository>();
        _mockProtocolRepository = new Mock<IProtocolRepository>();
        _mockTemplateRepository = new Mock<ITemplateRepository>();
        _mockUserMessageRepository = new Mock<IUserMessageRepository>();
        _mockAdditionalUserRepository = new Mock<IAdditionalUserRepository>();
        _mockUserLoginAttemptRepository = new Mock<IUserLoginAttemptRepository>();
        _mockEmailService = new Mock<IEmailService>();
        _mockMapper = new Mock<IMapper>();

        _controller = new UserController(
            _mockOrganizationRepository.Object,
            _mockProtocolRepository.Object,
            _mockRoleRepository.Object,
            _mockTemplateRepository.Object,
            _mockUserRepository.Object,
            _mockUserMessageRepository.Object,
            _mockAdditionalUserRepository.Object,
            _mockUserLoginAttemptRepository.Object,
            _mockEmailService.Object,
            _mockMapper.Object
        );

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

    // [Fact]
    // public void GetUsers_ReturnsOkResult_WithListOfUsers()
    // {
    //     // Arrange
    //     var users = new List<User>
    //     {
    //         new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Password = "password"},
    //         new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Password = "password" }
    //     };

    //     var userDtos = new List<UserDto>
    //     {
    //         new UserDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Username = "Johndoe" },
    //         new UserDto { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Username = "Janesmith" }
    //     };

    //     var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
    //     {
    //         new Claim(ClaimTypes.Name, "testuser"),
    //         new Claim(ClaimTypes.Role, "Admin"),
    //         new Claim("Organization", "1"),
    //         new Claim(ClaimTypes.NameIdentifier, "1"),
    //         new Claim("UserId", "1")
    //     }, "mock"));

    //     _mockUserRepository.Setup(repo => repo.GetUsers(It.IsAny<QueryObject>(), It.IsAny<UserSearchObject>(), claimsPrincipal)).Returns(users);
    //     _mockMapper.Setup(m => m.Map<List<UserDto>>(It.IsAny<List<User>>())).Returns(userDtos);

    //     // Act
    //     var result = _controller.GetUsers();

    //     // Assert
    //     var okResult = Assert.IsType<OkObjectResult>(result);
    //     var json = JsonConvert.SerializeObject(okResult.Value);
    //     var response = JsonConvert.DeserializeObject<PaginatedResponse<UserDto>>(json);

    //     Assert.NotNull(response);
    //     Assert.Equal(2, response.items.Count());
    //     Assert.Equal(1, response.items.First().Id);
    //     Assert.Equal("John", response.items.First().FirstName);
    // }


    [Fact]
    public void GetUser_ReturnsOkResult_WithUser()
    {
        // Arrange
        var userId = 1;
        var user = new User { Id = userId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Password = "password" };
        var userDto = new UserDto { Id = userId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Username = "Johndoe" };

        _mockUserRepository.Setup(repo => repo.UserExists(userId)).Returns(true);
        _mockUserRepository.Setup(repo => repo.GetUser(userId)).Returns(user);
        _mockMapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(userDto);

        // Act
        var result = _controller.GetUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value as UserDto;

        Assert.NotNull(response);
        Assert.Equal(userId, response.Id);
        Assert.Equal("John", response.FirstName);
    }

    [Fact]
    public void GetOrganizationsByUser_ReturnsOkResult_WithOrganizations()
    {
        // Arrange
        var userId = 1;
        var organizations = new List<Organization>
        {
            new Organization 
            { 
                Id = 1, 
                Name = "Organization1", 
                Address = "Address1", 
                PostalCode = "12345", 
                City = "City1", 
                OrganizationType = "Type1",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            },
            new Organization 
            { 
                Id = 2, 
                Name = "Organization2", 
                Address = "Address2", 
                PostalCode = "67890", 
                City = "City2", 
                OrganizationType = "Type2",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            }
        };
        var organizationDtos = new List<OrganizationDto>
        {
            new OrganizationDto 
            { 
                Id = 1, 
                Name = "Organization1", 
                Address = "Address1", 
                PostalCode = "12345", 
                City = "City1", 
                OrganizationType = "Type1",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            },
            new OrganizationDto 
            { 
                Id = 2, 
                Name = "Organization2", 
                Address = "Address2", 
                PostalCode = "67890", 
                City = "City2", 
                OrganizationType = "Type2",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            }
        };

        _mockUserRepository.Setup(repo => repo.UserExists(userId)).Returns(true);
        _mockOrganizationRepository.Setup(repo => repo.GetOrganizationsByUser(userId, It.IsAny<QueryObject>(), It.IsAny<OrganizationSearchObject>()))
            .Returns(organizations);
        _mockMapper.Setup(m => m.Map<List<OrganizationDto>>(It.IsAny<List<Organization>>()))
            .Returns(organizationDtos);

        // Act
        var result = _controller.GetOrganizationsByUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value as IEnumerable<OrganizationDto>;

        Assert.NotNull(response);
        Assert.Equal(2, response.Count());
        Assert.Equal(1, response.First().Id);
        Assert.Equal("Organization1", response.First().Name);
    }

    [Fact]
    public void GetProtocolsByUser_ReturnsOkResult_WithProtocols()
    {
        // Arrange
        var userId = 1;
        var protocols = new List<Protocol>
        {
            new Protocol 
            { 
                Id = 1, 
                Name = "Protocol1", 
                IsDraft = true, 
                IsInReview = false, 
                ReviewComment = null, 
                IsClosed = false, 
                ClosedAt = null, 
                sendEmail = false, 
                emailSubject = null, 
                emailContent = null, 
                CreatedDate = DateTime.Now, 
                UpdatedDate = DateTime.Now,
                User = new User { Id = userId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Password = "password" },
                Template = new Template { Id = 1, Name = "Template1", TemplateContent = "Content" }
            },
            new Protocol 
            { 
                Id = 2, 
                Name = "Protocol2", 
                IsDraft = false, 
                IsInReview = true, 
                ReviewComment = "Review needed", 
                IsClosed = false, 
                ClosedAt = null, 
                sendEmail = false, 
                emailSubject = null, 
                emailContent = null, 
                CreatedDate = DateTime.Now, 
                UpdatedDate = DateTime.Now,
                User = new User { Id = userId, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", Password = "password" },
                Template = new Template { Id = 2, Name = "Template2", TemplateContent = "Content" }
            }
        };
        var protocolDtos = new List<ProtocolDto>
        {
            new ProtocolDto 
            { 
                Id = 1, 
                Name = "Protocol1", 
                IsDraft = true, 
                IsInReview = false, 
                ReviewComment = null, 
                IsClosed = false, 
                ClosedAt = null, 
                sendEmail = false, 
                emailSubject = null, 
                emailContent = null, 
                CreatedDate = DateTime.Now, 
                UpdatedDate = DateTime.Now
            },
            new ProtocolDto 
            { 
                Id = 2, 
                Name = "Protocol2", 
                IsDraft = false, 
                IsInReview = true, 
                ReviewComment = "Review needed", 
                IsClosed = false, 
                ClosedAt = null, 
                sendEmail = false, 
                emailSubject = null, 
                emailContent = null, 
                CreatedDate = DateTime.Now, 
                UpdatedDate = DateTime.Now
            }
        };

        _mockUserRepository.Setup(repo => repo.UserExists(userId)).Returns(true);
        _mockProtocolRepository.Setup(repo => repo.GetProtocolsByUser(userId, It.IsAny<QueryObject>(), It.IsAny<ProtocolSearchObject>(), It.IsAny<ClaimsPrincipal>()))
            .Returns(protocols);
        _mockMapper.Setup(m => m.Map<List<ProtocolDto>>(It.IsAny<List<Protocol>>()))
            .Returns(protocolDtos);

        // Act
        var result = _controller.GetProtocolsByUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var json = JsonConvert.SerializeObject(okResult.Value);
        var response = JsonConvert.DeserializeObject<PaginatedResponse<ProtocolDto>>(json);


        Assert.NotNull(response);
        Assert.Equal(2, response.items.Count());
        Assert.Equal(1, response.items.First().Id);
        Assert.Equal("Protocol1", response.items.First().Name);
    }


    
}
