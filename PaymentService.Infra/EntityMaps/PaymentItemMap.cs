using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain.Entities;

namespace PaymentService.Infra.EntityMaps
{
    public class PaymentItemMap : IEntityTypeConfiguration<PaymentItem>
    {
        public void Configure(EntityTypeBuilder<PaymentItem> builder)
        {
            builder.ToTable("PaymentItems"); 

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();

            builder.Property(p => p.Sequence).IsRequired();
            builder.Property(p => p.Amount).IsRequired();

        }
    }
}
