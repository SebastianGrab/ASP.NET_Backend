using Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Models;
using Repository;

public class AdditionalUserRepositoryTests
{
    private readonly ProtocolContext _context;
    private readonly AdditionalUserRepository _repository;

    public AdditionalUserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ProtocolContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ProtocolContext(options);
        _repository = new AdditionalUserRepository(_context);

        ClearDatabase(); // Vor jedem Test die Datenbank bereinigen

        SeedDatabase();
    }

    private void ClearDatabase()
    {
        _context.Users.RemoveRange(_context.Users);
        _context.Templates.RemoveRange(_context.Templates);
        _context.Protocols.RemoveRange(_context.Protocols);
        _context.AdditionalUsers.RemoveRange(_context.AdditionalUsers);
        _context.SaveChanges();
    }

    private void SeedDatabase()
    {
        var user1 = new User
        {
            Id = 11,
            Email = "mail1@mail.de",
            Password = "password1",
            FirstName = "First1",
            LastName = "Last1",
            CreatedDate = DateTime.Now.AddDays(-10),
            UpdatedDate = DateTime.Now
        };

        var user2 = new User
        {
            Id = 12,
            Email = "mail2@mail.de",
            Password = "password2",
            FirstName = "First2",
            LastName = "Last2",
            CreatedDate = DateTime.Now.AddDays(-9),
            UpdatedDate = DateTime.Now
        };

        var template = new Template
        {
            Id = 1,
            Name = "Template1",
            TemplateContent = "TemplateContent1",
            CreatedDate = DateTime.Now.AddDays(-8),
            UpdatedDate = DateTime.Now
        };

        var protocols = new List<Protocol>
    {
        new Protocol
        {
            Id = 111,
            Name = "Protocol1",
            User = user1,
            Template = template,
            CreatedDate = DateTime.Now.AddDays(-7),
            UpdatedDate = DateTime.Now
        },
        new Protocol
        {
            Id = 222,
            Name = "Protocol2",
            User = user1,
            Template = template,
            CreatedDate = DateTime.Now.AddDays(-6),
            UpdatedDate = DateTime.Now
        },
        new Protocol
        {
            Id = 123,
            Name = "Protocol3",
            User = user2,
            Template = template,
            CreatedDate = DateTime.Now.AddDays(-5),
            UpdatedDate = DateTime.Now
        }
    };

        var additionalUsers = new List<AdditionalUser>
    {
        new AdditionalUser { User = user1, Protocol = protocols[0] },
        new AdditionalUser { User = user1, Protocol = protocols[1] },
        new AdditionalUser { User = user2, Protocol = protocols[2] }
    };

        _context.Protocols.AddRange(protocols);
        _context.AdditionalUsers.AddRange(additionalUsers);
        _context.SaveChanges();
    }

    [Fact]
    public void GetAdditionalUserEntriesByUser_ShouldReturnEntries_ForGivenUserId()
    {
        // Arrange
        var userId = 11;

        // Act
        var result = _repository.GetAdditionalUserEntriesByUser(userId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, au => Assert.Equal(userId, au.User.Id));
    }

    [Fact]
    public void GetAdditionalUserEntriesByProtocol_ShouldReturnEntries_ForGivenProtocolId()
    {
        // Arrange
        var protocolId = 111;

        // Act
        var result = _repository.GetAdditionalUserEntriesByProtocol(protocolId);

        // Assert
        Assert.Single(result);
        Assert.All(result, au => Assert.Equal(protocolId, au.Protocol.Id));
    }

    [Fact]
    public void AdditionalUserExists_ShouldReturnTrue_IfExists()
    {
        // Arrange
        var userId = 11;
        var protocolId = 111;

        // Act
        var exists = _repository.AdditionalUserExists(userId, protocolId);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public void AdditionalUserExists_ShouldReturnFalse_IfNotExists()
    {
        // Arrange
        var userId = 13;
        var protocolId = 133;

        // Act
        var exists = _repository.AdditionalUserExists(userId, protocolId);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public void DeleteAdditionalUserEntry_ShouldRemoveEntry_AndSaveChanges()
    {
        // Arrange
        var additionalUser = _context.AdditionalUsers.First();

        // Act
        var result = _repository.DeleteAdditionalUserEntry(additionalUser);

        // Assert
        Assert.True(result);
        Assert.DoesNotContain(additionalUser, _context.AdditionalUsers);
    }

    [Fact]
    public void DeleteAdditionalUserEntries_ShouldRemoveEntries_AndSaveChanges()
    {
        // Arrange
        var additionalUsers = _context.AdditionalUsers.ToList();

        // Act
        var result = _repository.DeleteAdditionalUserEntries(additionalUsers);

        // Assert
        Assert.True(result);
        Assert.Empty(_context.AdditionalUsers);
    }

    [Fact]
    public void GetAdditionalUser_ShouldReturnCorrectEntry()
    {
        // Arrange
        var userId = 11;
        var protocolId = 111;

        // Act
        var result = _repository.GetAdditionalUser(userId, protocolId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.User.Id);
        Assert.Equal(protocolId, result.Protocol.Id);
    }
}
