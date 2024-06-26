﻿// <auto-generated />
using System;
using Catalog.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Catalog.Infrastructure.Database.Migrations.CatalogMigrations
{
    [DbContext(typeof(CatalogContext))]
    [Migration("20220827063438_AddProductAttachment")]
    partial class AddProductAttachment
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Catalog.Domain.Core.AggregatesModel.FeatureAggregate.Feature", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.ToTable("Features", "Catalog");
                });

            modelBuilder.Entity("Catalog.Domain.Core.AggregatesModel.ProductAggregate.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Products", "Catalog");
                });

            modelBuilder.Entity("Catalog.Domain.Core.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Categories", "Catalog");
                });

            modelBuilder.Entity("Catalog.Domain.Core.AggregatesModel.ProductAggregate.Product", b =>
                {
                    b.OwnsMany("Catalog.Domain.Core.AggregatesModel.ProductAggregate.ProductAttachment", "ProductAttachments", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Extension")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("FileName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("FilePath")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("FileType")
                                .HasColumnType("int");

                            b1.Property<int>("Size")
                                .HasColumnType("int");

                            b1.HasKey("ProductId", "Id");

                            b1.ToTable("ProductAttachments", "Catalog");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.OwnsMany("Catalog.Domain.Core.AggregatesModel.ProductAggregate.ProductFeatureValue", "ProductFeatureValues", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("FeatureId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ProductId", "FeatureId");

                            b1.ToTable("ProductFeatureValues", "Catalog");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.Navigation("ProductAttachments");

                    b.Navigation("ProductFeatureValues");
                });

            modelBuilder.Entity("Catalog.Domain.Core.Category", b =>
                {
                    b.OwnsOne("Catalog.Domain.Core.AggregatesModel.CategoryAggregate.CategoryThumbnail", "Thumbnail", b1 =>
                        {
                            b1.Property<Guid>("CategoryId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Extension")
                                .IsRequired()
                                .HasMaxLength(16)
                                .HasColumnType("nvarchar(16)");

                            b1.Property<string>("FileName")
                                .IsRequired()
                                .HasMaxLength(128)
                                .HasColumnType("nvarchar(128)");

                            b1.Property<string>("FilePath")
                                .IsRequired()
                                .HasMaxLength(512)
                                .HasColumnType("nvarchar(512)");

                            b1.Property<int>("Size")
                                .HasColumnType("int");

                            b1.HasKey("CategoryId");

                            b1.ToTable("Categories", "Catalog");

                            b1.WithOwner()
                                .HasForeignKey("CategoryId");
                        });

                    b.OwnsMany("Catalog.Domain.Core.CategoryFeature", "CategoryFeatures", b1 =>
                        {
                            b1.Property<Guid>("CategoryId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("FeatureId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("CategoryId", "FeatureId");

                            b1.ToTable("CategoryFeatures", "Catalog");

                            b1.WithOwner()
                                .HasForeignKey("CategoryId");
                        });

                    b.Navigation("CategoryFeatures");

                    b.Navigation("Thumbnail")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
