﻿using Catalog.Domain.Core;
using Catalog.Domain.Core.AggregatesModel.FeatureAggregate;
using Catalog.Domain.Core.AggregatesModel.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Database.Configurations
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products", "Catalog");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
            .HasConversion(
                v => v.Value,
                v => new ProductId(v));

            builder.Property(x => x.CategoryId)
             .HasConversion(
            v => v.Value,
            v => new CategoryId(v));

            builder.Property(e => e.Title)
            .HasMaxLength(256).IsRequired();

            builder.Property(e => e.Code)
          .HasMaxLength(16).IsRequired();

            builder.OwnsMany(s => s.ProductFeatureValues, y =>
            {
                y.ToTable("ProductFeatureValues", "Catalog");

                y.WithOwner().HasForeignKey("ProductId");
                //y.WithOwner().HasForeignKey("FeatureId");

                y.Property(x => x.ProductId)
                .HasConversion(
                    v => v.Value,
                    v => new ProductId(v));

                y.Property(x => x.FeatureId)
                .HasConversion(
                    v => v.Value,
                    v => new FeatureId(v));

                y.HasKey("ProductId", "FeatureId");

            });

            builder.OwnsMany(s => s.ProductAttachments, y =>
            {
                 builder.HasKey(x => x.Id);
                y.WithOwner().HasForeignKey("ProductId");

                y.ToTable("ProductAttachments", "Catalog");

                y.Property(x => x.ProductId)
                .HasConversion(
                    v => v.Value,
                    v => new ProductId(v));
            });
        }
    }
}
