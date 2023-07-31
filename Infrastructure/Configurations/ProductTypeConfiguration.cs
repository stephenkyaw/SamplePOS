using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.HasKey(x => x.ProductTypeId);


            builder.HasMany(x => x.Products).WithOne(x => x.ProductType).HasForeignKey(x => x.ProductTypeId);
        }
    }
}
