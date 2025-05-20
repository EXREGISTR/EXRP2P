using Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order> {
    public void Configure(EntityTypeBuilder<Order> builder) {
        builder.ToTable("orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PaymentBanks)
            .HasConversion(
                banksArray => string.Join(',', banksArray),
                banksString => banksString.Split(',', StringSplitOptions.None));

        builder.HasIndex(x => x.CreatorId);
        builder.HasIndex(x => x.Status);
        
    }
}
