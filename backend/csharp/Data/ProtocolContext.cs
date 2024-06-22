using Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using csharp.Models;

namespace Data
{
    public class ProtocolContext : DbContext
    {
        public ProtocolContext(DbContextOptions<ProtocolContext> options) : base(options)
        {
        }

        public DbSet<AdditionalUser> AdditionalUsers { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<Protocol> Protocols { get; set; }

        public DbSet<ProtocolContent> ProtocolContents { get; set; }

        public DbSet<ProtocolPdfFile> ProtocolPdfFiles { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Template> Templates { get; set; }

        public DbSet<TemplateOrganization> TemplateOrganizations { get; set; }

        public DbSet<User> Users  { get; set; }

        public DbSet<UserOrganizationRole> UserOrganizationRoles  { get; set; }

        public DbSet<UserMessage> UserMessages  { get; set; }

        public DbSet<UserLoginAttempt> UserLoginAttempts  { get; set; }

        public DbSet<TemplateVersions> TemplateVersions  { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdditionalUser>()
                .HasKey(au => new { au.userId, au.protocolId });
            modelBuilder.Entity<AdditionalUser>()
                .HasOne(e => e.User)
                .WithMany(e => e.AdditionalUser)
                .HasForeignKey(e => e.userId);
            modelBuilder.Entity<AdditionalUser>()
                .HasOne(e => e.Protocol)
                .WithMany(e => e.AdditionalUser)
                .HasForeignKey(e => e.protocolId);

            
            modelBuilder.Entity<TemplateOrganization>()
                    .HasKey(ta => new { ta.organizationId, ta.templateId });
            modelBuilder.Entity<TemplateOrganization>()
                    .HasOne(t => t.Template)
                    .WithMany(ta => ta.TemplateOrganizations)
                    .HasForeignKey(t => t.templateId);
            modelBuilder.Entity<TemplateOrganization>()
                    .HasOne(o => o.Organization)
                    .WithMany(ta => ta.TemplateOrganizations)
                    .HasForeignKey(o => o.organizationId);


            modelBuilder.Entity<UserOrganizationRole>()
                    .HasKey(ur => new { ur.userId, ur.roleId, ur.organizationId });
            modelBuilder.Entity<UserOrganizationRole>()
                    .HasOne(u => u.User)
                    .WithMany(ur => ur.UserOrganizationRoles)
                    .HasForeignKey(u => u.userId);
            modelBuilder.Entity<UserOrganizationRole>()
                    .HasOne(r => r.Role)
                    .WithMany(ur => ur.UserOrganizationRoles)
                    .HasForeignKey(r => r.roleId);
            modelBuilder.Entity<UserOrganizationRole>()
                    .HasOne(o => o.Organization)
                    .WithMany(ur => ur.UserOrganizationRoles)
                    .HasForeignKey(o => o.organizationId);

            // Seeding:
            modelBuilder.Entity<Role>()
                    .HasData(
                        new Role() { Id = -1, Name = "Helfer" },
                        new Role() { Id = -2, Name = "Leiter" },
                        new Role() { Id = -3, Name = "Admin" }
                    );

                    
            modelBuilder.Entity<Organization>()
                    .HasData(
                        new Organization() { Id = -1, Name = "Deutsches Rotes Kreuz e.V.", OrganizationType = "Bundesorganisation", Address = "Carstennstraße 58", City = "Berlin", PostalCode = "12205", CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow },
                        new Organization() { Id = -2, parentId = -1, Name = "Test Tochter 1", OrganizationType = "Bundesorganisation", Address = "Carstennstraße 58", City = "Berlin", PostalCode = "12205", CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow },
                        new Organization() { Id = -3, parentId = -2, Name = "Test Tochter 2", OrganizationType = "Bundesorganisation", Address = "Carstennstraße 58", City = "Berlin", PostalCode = "12205", CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow }
                    );

                    
            modelBuilder.Entity<User>()
                    .HasData(
                        new User() { Id = -1, FirstName = "Super", LastName = "Admin", Email = "superadmin@drk.de", Password = BCrypt.Net.BCrypt.HashPassword("SuperAdminPasswort"), CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, LastPasswordChangeDate = DateTime.UtcNow },
                        new User() { Id = -2, FirstName = "Test", LastName = "Helfer", Email = "testhelfer@drk.de", Password = BCrypt.Net.BCrypt.HashPassword("TestHelferPasswort"), CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, LastPasswordChangeDate = DateTime.UtcNow },
                        new User() { Id = -3, FirstName = "Test", LastName = "Leiter", Email = "testleiter@drk.de", Password = BCrypt.Net.BCrypt.HashPassword("TestLeiterPasswort"), CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, LastPasswordChangeDate = DateTime.UtcNow }
                    );

                    
            modelBuilder.Entity<UserOrganizationRole>()
                    .HasData(
                        new UserOrganizationRole() { Id = -1, organizationId = -1, roleId = -3, userId = -1, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow },
                        new UserOrganizationRole() { Id = -2, organizationId = -3, roleId = -1, userId = -2, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow },
                        new UserOrganizationRole() { Id = -3, organizationId = -2, roleId = -2, userId = -3, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow }
                    );

                    
            modelBuilder.Entity<UserLoginAttempt>()
                    .HasData(
                        new UserLoginAttempt() { Id = -1, userId = -1 },
                        new UserLoginAttempt() { Id = -2, userId = -2 },
                        new UserLoginAttempt() { Id = -3, userId = -3 }
                    );

                    
            modelBuilder.Entity<Template>()
                    .HasData(
                        new Template() { Id = -1, Name = "Standard-Template", organizationId = -1, Description = "Standard-Template für alle Organisationen", TemplateContent = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data/Seeding/DefaultTemplate.txt").Replace(@".Tests\bin\Debug\net7.0", "")), CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow }
                    );

                    
            modelBuilder.Entity<TemplateVersions>()
                    .HasData(
                        new TemplateVersions() { Id = -1, TemplateContent = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data/Seeding/DefaultTemplate.txt").Replace(@".Tests\bin\Debug\net7.0", "")), templateId = -1, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow }
                    );
        }


        // Update change tracker
        public override int SaveChanges()
        {
            UpdateTimestamps();
            UpdatePasswordData();
            UpdateProtocolData();
            return base.SaveChanges();
        }

        private void UpdateTimestamps()
            {
                var entries = ChangeTracker
                    .Entries()
                    .Where(e => e.Entity is ITimestampEntity && 
                                (e.State == EntityState.Added || e.State == EntityState.Modified));

                foreach (var entityEntry in entries)
                {
                    var entity = (ITimestampEntity)entityEntry.Entity;
                    entity.UpdatedDate = DateTime.UtcNow;

                    if (entityEntry.State == EntityState.Added)
                    {
                        entity.CreatedDate = DateTime.UtcNow;
                    }
                    else if (entityEntry.State == EntityState.Modified)
                    {
                        entityEntry.Property("CreatedDate").IsModified = false;
                    }
                }
            }

        private void UpdatePasswordData()
            {
                var entries = ChangeTracker
                    .Entries<User>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

                    foreach (var entityEntry in entries)
                    {
                        var entity = (User)entityEntry.Entity;

                    if (entityEntry.State == EntityState.Added)
                    {
                        entityEntry.Entity.LastPasswordChangeDate = DateTime.UtcNow;
                    }
                    else if (entityEntry.State == EntityState.Modified)
                    {
                        if (entityEntry.Property("Password").IsModified)
                        {
                            entityEntry.Entity.LastPasswordChangeDate = DateTime.UtcNow;
                        }
                        else {
                        entityEntry.Property("LastPasswordChangeDate").IsModified = false;
                        }
                    }
                }
            }

        private void UpdateProtocolData()
            {
                var entries = ChangeTracker
                    .Entries<Protocol>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

                foreach (var entityEntry in entries)
                {
                    var entity = (Protocol)entityEntry.Entity;

                    if (entityEntry.State == EntityState.Added)
                    {
                        entity.IsClosed = false;
                        entity.ClosedAt = null;
                    }
                    else if (entityEntry.State == EntityState.Modified && entityEntry.OriginalValues["IsClosed"] != entityEntry.CurrentValues["IsClosed"])
                    {
                        entity.ClosedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("ClosedAt").IsModified = false;
                    }
                }
            }


        // ConnectionString confiuration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = WebApplication.CreateBuilder();
            var configuration = builder.Configuration;
            if(!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("ConnectionString"));
        }
    }
}
