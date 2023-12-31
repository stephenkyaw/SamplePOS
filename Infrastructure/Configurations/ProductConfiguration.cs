﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.ProductId);

            builder.Property(p => p.SellingPrice).HasPrecision(18, 2);
            builder.Property(p => p.BuyingPrice).HasPrecision(18, 2);

            builder.HasOne<ProductType>(x => x.ProductType).WithMany(x => x.Products).HasForeignKey(x => x.ProductTypeId);

            builder.HasMany<SaleOrderItem>(x => x.SaleOrderItems).WithOne(x => x.Product).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
