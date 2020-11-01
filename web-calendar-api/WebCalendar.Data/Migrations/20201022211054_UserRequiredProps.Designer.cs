﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebCalendar.Data;

namespace WebCalendar.Data.Migrations
{
    [DbContext(typeof(WebCalendarDbContext))]
    [Migration("20201022211054_UserRequiredProps")]
    partial class UserRequiredProps
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebCalendar.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "user1@mail.com",
                            FirstName = "FN1",
                            LastName = "LN1",
                            PasswordHash = new byte[] { 1, 2, 3, 4, 5 },
                            Salt = new byte[] { 1, 2 }
                        },
                        new
                        {
                            Id = 2,
                            Email = "user2@mail.com",
                            FirstName = "FN2",
                            LastName = "LN2",
                            PasswordHash = new byte[] { 1, 2, 3, 4, 5 },
                            Salt = new byte[] { 1, 2 }
                        },
                        new
                        {
                            Id = 3,
                            Email = "user3@mail.com",
                            FirstName = "FN3",
                            LastName = "LN3",
                            PasswordHash = new byte[] { 1, 2, 3, 4, 5 },
                            Salt = new byte[] { 1, 2 }
                        });
                });
#pragma warning restore 612, 618
        }
    }
}