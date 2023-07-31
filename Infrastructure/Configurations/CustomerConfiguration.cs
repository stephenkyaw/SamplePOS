using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.CustomerId);
            builder.HasMany(x => x.SaleOrders).WithOne(x => x.Customer);

            builder.Property(p => p.TotalPoints).HasPrecision(18, 2);
            builder.Property(p => p.TotalPurchasedAmount).HasPrecision(18, 2);
        }
    }

   

   
}
