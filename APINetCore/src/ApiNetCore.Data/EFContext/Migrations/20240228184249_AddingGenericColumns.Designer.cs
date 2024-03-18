﻿// <auto-generated />
using System;
using ApiNetCore.Data.EFContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApiNetCore.Data.EFContext.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240228184249_AddingGenericColumns")]
    partial class AddingGenericColumns
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ApiNetCore.Business.Models.Band", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageFileName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("LastChangeAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("MusicalStyles")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("Revoked")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Band");
                });

            modelBuilder.Entity("ApiNetCore.Business.Models.Musician", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("Date");

                    b.Property<DateTime>("LastChangeAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PictureFileName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("Revoked")
                        .HasColumnType("bit");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Surnames")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Musician");
                });

            modelBuilder.Entity("BandMusician", b =>
                {
                    b.Property<int>("BandsId")
                        .HasColumnType("int");

                    b.Property<int>("MusiciansId")
                        .HasColumnType("int");

                    b.HasKey("BandsId", "MusiciansId");

                    b.HasIndex("MusiciansId");

                    b.ToTable("BandMusician");
                });

            modelBuilder.Entity("BandMusician", b =>
                {
                    b.HasOne("ApiNetCore.Business.Models.Band", null)
                        .WithMany()
                        .HasForeignKey("BandsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiNetCore.Business.Models.Musician", null)
                        .WithMany()
                        .HasForeignKey("MusiciansId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
