using Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Models;
using Repository;

public class OrganizationRepositoryTests
{
    private readonly ProtocolContext _context;
    private readonly OrganizationRepository _repository;

    public OrganizationRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ProtocolContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ProtocolContext(options);
        _repository = new OrganizationRepository(_context);

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        _context.Organizations.RemoveRange(_context.Organizations);
        _context.SaveChanges();
        var organizations = new List<Organization>
        {
            new Organization { Id = 1, Name = "Org1", CreatedDate = DateTime.Now.AddDays(-10), UpdatedDate = DateTime.Now, OrganizationType = "Type1" },
            new Organization { Id = 2, Name = "Org2", CreatedDate = DateTime.Now.AddDays(-5), UpdatedDate = DateTime.Now, OrganizationType = "Type2" },
            // Add more organizations if needed
        };

        _context.Organizations.AddRange(organizations);
        _context.SaveChanges();
    }


    [Fact]
    public void GetOrganization_ReturnsCorrectOrganization()
    {
        var result = _repository.GetOrganization(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void OrganizationExists_ReturnsTrueIfExists()
    {
        var result = _repository.OrganizationExists(1);

        Assert.True(result);
    }

    [Fact]
    public void OrganizationExists_ReturnsFalseIfNotExists()
    {
        var result = _repository.OrganizationExists(999);

        Assert.False(result);
    }

    [Fact]
    public void CreateOrganization_SavesToDatabase()
    {
        var newOrganization = new Organization
        {
            Id = 3,
            Name = "Org3",
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            OrganizationType = "Type3"
        };

        var result = _repository.CreateOrganization(newOrganization);

        Assert.True(result);
        Assert.Equal(3, _context.Organizations.Count());
    }

    [Fact]
    public void UpdateOrganization_UpdatesInDatabase()
    {
        var organization = _context.Organizations.First();
        organization.Name = "UpdatedName";

        var result = _repository.UpdateOrganization(organization);

        Assert.True(result);
        Assert.Equal("UpdatedName", _context.Organizations.First().Name);
    }

    [Fact]
    public void DeleteOrganization_RemovesFromDatabase()
    {
        var organization = _context.Organizations.First();

        var result = _repository.DeleteOrganization(organization);

        Assert.True(result);
        Assert.Equal(1, _context.Organizations.Count());
    }
}
