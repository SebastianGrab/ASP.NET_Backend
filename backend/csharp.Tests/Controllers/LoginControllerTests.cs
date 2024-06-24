using Moq;
using Xunit;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Controllers;
using Interfaces;
using Models;
using csharp.Interfaces;
using Helper;
using Helper.SeachObjects;

public class LoginControllerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IOrganizationRepository> _mockOrganizationRepository;
    private readonly Mock<IRoleRepository> _mockRoleRepository;
    private readonly Mock<ILogger<LoginController>> _mockLogger;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IPasswordGenerator> _mockPasswordGenerator;
    private readonly Mock<IAuthenticationService> _mockAuthenticationService;
    private readonly LoginController _controller;

    public LoginControllerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockOrganizationRepository = new Mock<IOrganizationRepository>();
        _mockRoleRepository = new Mock<IRoleRepository>();
        _mockLogger = new Mock<ILogger<LoginController>>();
        _mockMapper = new Mock<IMapper>();
        _mockEmailService = new Mock<IEmailService>();
        _mockPasswordGenerator = new Mock<IPasswordGenerator>();
        _mockAuthenticationService = new Mock<IAuthenticationService>();

        _controller = new LoginController(
            _mockUserRepository.Object,
            _mockRoleRepository.Object,
            _mockPasswordGenerator.Object,
            _mockEmailService.Object,
            _mockOrganizationRepository.Object,
            _mockLogger.Object,
            _mockMapper.Object,
            _mockAuthenticationService.Object
        );
    }

    [Fact]
    public void Login_ReturnsOkResult_WithLoginReturnObject()
    {
        // Arrange
        var loginObject = new LoginObject { Email = "test@example.com", Password = "password" };
        var user = new User { Id = 1, Email = "test@example.com", Password = "hashedpassword" };
        var token = "testtoken";
        var organization = new Organization 
        { 
            Id = 1, 
            Name = "Organization1", 
            OrganizationType = "Type1", 
            Address = "123 Street", 
            PostalCode = "12345", 
            City = "City1", 
            CreatedDate = DateTime.UtcNow, 
            UpdatedDate = DateTime.UtcNow 
        };
        var role = new Role { Id = 1, Name = "Admin" };
        var uor = new List<UserOrganizationRole>
        {
            new UserOrganizationRole () {
            Id = 1, 
            organizationId = 1,
            roleId = role.Id,
            CreatedDate = DateTime.UtcNow, 
            UpdatedDate = DateTime.UtcNow 
            }
        };

        _mockUserRepository.Setup(repo => repo.GetUserByEmail(loginObject.Email)).Returns(user);
        _mockUserRepository.Setup(repo => repo.GetUserOrganizationRoleEntriesByUser(user.Id)).Returns(uor);
        _mockAuthenticationService.Setup(auth => auth.Login(loginObject)).Returns(token);
        _mockOrganizationRepository.Setup(repo => repo.GetOrganizationsByUser(user.Id, It.IsAny<QueryObject>(), It.IsAny<OrganizationSearchObject>())).Returns(new List<Organization> { organization });
        _mockRoleRepository.Setup(repo => repo.GetRolesByUser(user.Id)).Returns(new List<Role> { role });

        // Act
        var result = _controller.Login(loginObject);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<LoginReturnObject>(okResult.Value);
        Assert.Equal(token, returnValue.Token);
        Assert.Equal(user.Id, returnValue.userId);
        Assert.Equal(organization.Id, returnValue.organizationId);
        Assert.Equal(role.Name, returnValue.role);
    }


    [Fact]
    public void UpdateUser_ReturnsOk_WhenUserUpdated()
    {
        // Arrange
        var userEmail = "test@example.com";
        var user = new User { Id = 1, Email = userEmail, FirstName = "John", LastName = "Doe", Password = "password" };
        var newPassword = "newpassword";

        _mockUserRepository.Setup(repo => repo.GetUserByEmail(userEmail)).Returns(user);
        _mockPasswordGenerator.Setup(gen => gen.GetRandomAlphanumericString(12)).Returns(newPassword);
        _mockEmailService.Setup(service => service.SendResetEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _mockUserRepository.Setup(repo => repo.UpdateUser(It.IsAny<User>())).Returns(true);
        _mockMapper.Setup(m => m.Map<User>(It.IsAny<User>())).Returns(user);

        // Act
        var result = _controller.UpdateUser(userEmail);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void UpdateUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userEmail = "test@example.com";

        _mockUserRepository.Setup(repo => repo.GetUserByEmail(userEmail)).Returns((User)null);

        // Act
        var result = _controller.UpdateUser(userEmail);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void UpdateUser_ReturnsBadRequest_WhenEmailServiceFails()
    {
        // Arrange
        var userEmail = "test@example.com";
        var user = new User { Id = 1, Email = userEmail, FirstName = "John", LastName = "Doe", Password = "password" };
        var newPassword = "newpassword";

        _mockUserRepository.Setup(repo => repo.GetUserByEmail(userEmail)).Returns(user);
        _mockPasswordGenerator.Setup(gen => gen.GetRandomAlphanumericString(12)).Returns(newPassword);
        _mockEmailService.Setup(service => service.SendResetEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        // Act
        var result = _controller.UpdateUser(userEmail);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(301, statusCodeResult.StatusCode);
    }

    [Fact]
    public void UpdateUser_ReturnsStatusCode_WhenUpdateFails()
    {
        // Arrange
        var userEmail = "test@example.com";
        var user = new User { Id = 1, Email = userEmail, FirstName = "John", LastName = "Doe", Password = "password" };
        var newPassword = "newpassword";

        _mockUserRepository.Setup(repo => repo.GetUserByEmail(userEmail)).Returns(user);
        _mockPasswordGenerator.Setup(gen => gen.GetRandomAlphanumericString(12)).Returns(newPassword);
        _mockEmailService.Setup(service => service.SendResetEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _mockUserRepository.Setup(repo => repo.UpdateUser(It.IsAny<User>())).Returns(false);
        _mockMapper.Setup(m => m.Map<User>(It.IsAny<User>())).Returns(user);

        // Act
        var result = _controller.UpdateUser(userEmail);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
}
