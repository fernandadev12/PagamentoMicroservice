using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain.Entities;

namespace PaymentService.Infra.EntityMaps
{
    public class PaymentMap : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Amount)
                    .HasColumnName("TotalAmount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            builder.Property(p => p.Discount);
            builder.Property(p => p.Installments);

            builder.OwnsOne(p => p.OriginalAmount, money =>
            {
                money.Property(m => m.Value)
                     .HasColumnName("OriginalAmountValue")
                     .HasColumnType("decimal(18,2)")
                     .IsRequired();
            });
            builder.OwnsOne(p => p.FinalAmount, money =>
            {
                money.Property(m => m.Value)
                    .HasColumnName("FinalAmountValue")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            });

            builder
                 .HasOne(p => p.PaymentMethodId)
                 .WithOne(pm => pm.PaymentId)
                 .HasForeignKey<Payment>("PaymentMethodId");

            // Relacionamento Um-para-Muitos (Payment -> PaymentItem)
            builder
                .HasMany(p => p.Items)
                .WithOne()
                .IsRequired()
                .HasForeignKey("PaymentId");

            //Acesso ao campo privado para a coleção Items
            var itens = builder.Metadata.FindNavigation(nameof(Payment.Items));
            if (itens != null)
            {
                itens.SetPropertyAccessMode(PropertyAccessMode.Field);
            }

        }
    }
}
