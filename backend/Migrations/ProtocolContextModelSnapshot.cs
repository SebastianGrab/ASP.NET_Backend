﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(ProtocolContext))]
    partial class ProtocolContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Models.AdditionalUser", b =>
                {
                    b.Property<long>("userId")
                        .HasColumnType("bigint");

                    b.Property<long>("protocolId")
                        .HasColumnType("bigint");

                    b.HasKey("userId", "protocolId");

                    b.HasIndex("protocolId");

                    b.ToTable("AdditionalUsers");
                });

            modelBuilder.Entity("Models.Organization", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OrganizationType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("parentId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Models.Protocol", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("ClosedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDraft")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("OrganizationId")
                        .HasColumnType("bigint");

                    b.Property<string>("ReviewComment")
                        .HasColumnType("text");

                    b.Property<long>("TemplateId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("TemplateId");

                    b.HasIndex("UserId");

                    b.ToTable("Protocols");
                });

            modelBuilder.Entity("Models.ProtocolContent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("protocolId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("protocolId")
                        .IsUnique();

                    b.ToTable("ProtocolContents");
                });

            modelBuilder.Entity("Models.ProtocolPdfFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("protocolId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("protocolId")
                        .IsUnique();

                    b.ToTable("ProtocolPdfFiles");
                });

            modelBuilder.Entity("Models.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "Helfer"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "Einsatzformationsleiter"
                        },
                        new
                        {
                            Id = 3L,
                            Name = "Bereitschaftsleiter"
                        },
                        new
                        {
                            Id = 4L,
                            Name = "Kreisverbandsleiter"
                        },
                        new
                        {
                            Id = 5L,
                            Name = "Admin"
                        });
                });

            modelBuilder.Entity("Models.Template", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("OrganizationId")
                        .HasColumnType("bigint");

                    b.Property<string>("TemplateContent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("Models.TemplateOrganization", b =>
                {
                    b.Property<long>("organizationId")
                        .HasColumnType("bigint");

                    b.Property<long>("templateId")
                        .HasColumnType("bigint");

                    b.HasKey("organizationId", "templateId");

                    b.HasIndex("templateId");

                    b.ToTable("TemplateOrganizations");
                });

            modelBuilder.Entity("Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastPasswordChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Models.UserMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SentFrom")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserMessages");
                });

            modelBuilder.Entity("Models.UserOrganizationRole", b =>
                {
                    b.Property<long>("userId")
                        .HasColumnType("bigint");

                    b.Property<long>("roleId")
                        .HasColumnType("bigint");

                    b.Property<long>("organizationId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("userId", "roleId", "organizationId");

                    b.HasIndex("organizationId");

                    b.HasIndex("roleId");

                    b.ToTable("UserOrganizationRoles");
                });

            modelBuilder.Entity("Models.UserSession", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("Models.AdditionalUser", b =>
                {
                    b.HasOne("Models.Protocol", "Protocol")
                        .WithMany("AdditionalUser")
                        .HasForeignKey("protocolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.User", "User")
                        .WithMany("AdditionalUser")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Protocol");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.Protocol", b =>
                {
                    b.HasOne("Models.Organization", "Organization")
                        .WithMany("Protocols")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Template", "Template")
                        .WithMany("Protocols")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.User", "User")
                        .WithMany("Protocols")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("Template");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.ProtocolContent", b =>
                {
                    b.HasOne("Models.Protocol", "Protocol")
                        .WithOne("ProtocolContent")
                        .HasForeignKey("Models.ProtocolContent", "protocolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Protocol");
                });

            modelBuilder.Entity("Models.ProtocolPdfFile", b =>
                {
                    b.HasOne("Models.Protocol", "Protocol")
                        .WithOne("ProtocolPdfFile")
                        .HasForeignKey("Models.ProtocolPdfFile", "protocolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Protocol");
                });

            modelBuilder.Entity("Models.Template", b =>
                {
                    b.HasOne("Models.Organization", "Organization")
                        .WithMany("Templates")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Models.TemplateOrganization", b =>
                {
                    b.HasOne("Models.Organization", "Organization")
                        .WithMany("TemplateOrganizations")
                        .HasForeignKey("organizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Template", "Template")
                        .WithMany("TemplateOrganizations")
                        .HasForeignKey("templateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("Template");
                });

            modelBuilder.Entity("Models.UserMessage", b =>
                {
                    b.HasOne("Models.User", "User")
                        .WithMany("UserMessages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.UserOrganizationRole", b =>
                {
                    b.HasOne("Models.Organization", "Organization")
                        .WithMany("UserOrganizationRoles")
                        .HasForeignKey("organizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Role", "Role")
                        .WithMany("UserOrganizationRoles")
                        .HasForeignKey("roleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.User", "User")
                        .WithMany("UserOrganizationRoles")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.UserSession", b =>
                {
                    b.HasOne("Models.User", "User")
                        .WithMany("UserSessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.Organization", b =>
                {
                    b.Navigation("Protocols");

                    b.Navigation("TemplateOrganizations");

                    b.Navigation("Templates");

                    b.Navigation("UserOrganizationRoles");
                });

            modelBuilder.Entity("Models.Protocol", b =>
                {
                    b.Navigation("AdditionalUser");

                    b.Navigation("ProtocolContent")
                        .IsRequired();

                    b.Navigation("ProtocolPdfFile")
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Role", b =>
                {
                    b.Navigation("UserOrganizationRoles");
                });

            modelBuilder.Entity("Models.Template", b =>
                {
                    b.Navigation("Protocols");

                    b.Navigation("TemplateOrganizations");
                });

            modelBuilder.Entity("Models.User", b =>
                {
                    b.Navigation("AdditionalUser");

                    b.Navigation("Protocols");

                    b.Navigation("UserMessages");

                    b.Navigation("UserOrganizationRoles");

                    b.Navigation("UserSessions");
                });
#pragma warning restore 612, 618
        }
    }
}
