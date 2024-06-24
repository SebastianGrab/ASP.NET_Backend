using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Xunit;

namespace Repository.Tests
{
    public class ProtocolContentRepositoryTests
    {
        private readonly ProtocolContext _context;
        private readonly ProtocolContentRepository _repository;

        public ProtocolContentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ProtocolContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ProtocolContext(options);
            _repository = new ProtocolContentRepository(_context);

            ClearDatabase();

            SeedDatabase();
        }

        private void ClearDatabase()
        {
            _context.Users.RemoveRange(_context.Users);
            _context.Templates.RemoveRange(_context.Templates);
            _context.Protocols.RemoveRange(_context.Protocols);
            _context.AdditionalUsers.RemoveRange(_context.AdditionalUsers);
            _context.ProtocolContents.RemoveRange(_context.ProtocolContents);
            _context.ProtocolPdfFiles.RemoveRange(_context.ProtocolPdfFiles);
            _context.Organizations.RemoveRange(_context.Organizations);
            _context.SaveChanges();
        }

        private void SeedDatabase()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var organization = new Organization
            {
                Id = 1001,
                Name = "Org1",
                Address = "123 Main St",
                PostalCode = "12345",
                City = "City1",
                OrganizationType = "Type1",
                CreatedDate = DateTime.Now.AddDays(-10),
                UpdatedDate = DateTime.Now
            };

            var user1 = new User
            {
                Id = 2001,
                Email = "mail1@mail.de",
                Password = "password1",
                FirstName = "First1",
                LastName = "Last1",
                CreatedDate = DateTime.Now.AddDays(-9),
                UpdatedDate = DateTime.Now
            };

            var template = new Template
            {
                Id = 3001,
                Name = "Template 1",
                TemplateContent = "Template 1 Content",
                CreatedDate = DateTime.Now.AddDays(-8),
                UpdatedDate = DateTime.Now
            };

            var protocol = new Protocol
            {
                Id = 4001,
                Name = "Protocol 1",
                User = user1,
                Template = template,
                Organization = organization,
                CreatedDate = DateTime.Now.AddDays(-7),
                UpdatedDate = DateTime.Now
            };

            var protocolPdfFile = new ProtocolPdfFile
            {
                Id = 5001,
                Content = "Dummy PDF Content 1",
                MimeType = "application/pdf",
                protocolId = 4001,
                CreatedDate = DateTime.Now.AddDays(-6),
                UpdatedDate = DateTime.Now
            };

            var protocolContent = new ProtocolContent
            {
                Id = 6001,
                Protocol = protocol,
                Content = "Protocol 1 Content",
                CreatedDate = DateTime.Now.AddDays(-5),
                UpdatedDate = DateTime.Now
            };

            _context.Organizations.Add(organization);
            _context.Users.Add(user1);
            _context.Templates.Add(template);
            _context.Protocols.Add(protocol);
            _context.ProtocolPdfFiles.Add(protocolPdfFile);
            _context.ProtocolContents.Add(protocolContent);
            _context.SaveChanges();
        }

        [Fact]
        public void UpdateProtocolContent_ShouldUpdateInDatabase()
        {
            // Arrange
            var protocolContent = _context.ProtocolContents.First();
            protocolContent.Content = "Updated Content";

            // Act
            var result = _repository.UpdateProtocolContent(protocolContent);

            // Assert
            Assert.True(result);
            Assert.Equal("Updated Content", _context.ProtocolContents.First().Content);
        }

        [Fact]
        public void DeleteProtocolContent_ShouldRemoveFromDatabase()
        {
            // Arrange
            var protocolContent = _context.ProtocolContents.First();

            // Act
            var result = _repository.DeleteProtocolContent(protocolContent);

            // Assert
            Assert.True(result);
            Assert.Equal(0, _context.ProtocolContents.Count());
        }


        [Fact]
        public void ProtocolContentExists_ShouldReturnFalse_IfNotExists()
        {
            // Arrange
            var nonExistingProtocolContentId = 9999;

            // Act
            var result = _repository.ProtocolContentExists(nonExistingProtocolContentId);

            // Assert
            Assert.False(result);
        }
    }
}
