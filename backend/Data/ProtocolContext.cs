using Models;
using Microsoft.EntityFrameworkCore;

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

        public DbSet<UserSession> UserSessions  { get; set; }

        public DbSet<UserMessage> UserMessages  { get; set; }



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
                        new Role() { Id = 1, Name = "Helfer" },
                        new Role() { Id = 2, Name = "Einsatzformationsleiter" },
                        new Role() { Id = 3, Name = "Bereitschaftsleiter" },
                        new Role() { Id = 4, Name = "Kreisverbandsleiter" },
                        new Role() { Id = 5, Name = "Admin" }
                    );

                    
            modelBuilder.Entity<Organization>()
                    .HasData(
                        new Organization() { Id = 1, Name = "Testorganisation", OrganizationType = "Test" }
                    );

                    
            modelBuilder.Entity<User>()
                    .HasData(
                        new User() { Id = 1, FirstName = "Super", LastName = "Admin", Email = "superadmin@drk.de", Password = BCrypt.Net.BCrypt.HashPassword("SuperAdminPasswort") }
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
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("ConnectionString"));
        }
    }
}
