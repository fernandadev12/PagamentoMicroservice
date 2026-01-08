using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain.Entities;
using PaymentService.Domain.ValueObjects;

namespace PaymentService.Infra.EntityMaps
{
    public class PaymentMap : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.OriginalAmount)
               .HasConversion(
                   m => m.Value,          // salva só o decimal
                   v => new Money(v)      // reconstrói ao ler
               )
               .HasColumnName("OriginalAmountValue")
               .HasColumnType("decimal(18,2)")
               .IsRequired();


            builder.Property(p => p.Discount)
               .HasConversion(
                   d => d.Percent,          // como salvar no banco
                   v => new Discount(v)   // como reconstruir ao ler
               )
               .HasColumnName("DiscountValue")
               .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Installments)
                   .HasConversion(
                       i => i.Count,                // como salvar no banco (int)
                       v => new Installments(v)     // como reconstruir ao ler
                   )
                   .HasColumnName("InstallmentsCount")
                   .HasColumnType("int")
                   .IsRequired();


            builder.OwnsOne(p => p.OriginalAmount, money =>
            {
                money.Property(m => m.Value)
                     .HasColumnName("OriginalAmountValue")
                     .HasColumnType("decimal(18,2)")
                     .IsRequired();
            });

            builder.Property(p => p.FinalAmount)
               .HasConversion(
                   m => m.Value,
                   v => new Money(v)
               )
               .HasColumnName("FinalAmountValue")
               .HasColumnType("decimal(18,2)")
               .IsRequired();

            builder.HasOne(p => p.PaymentMethod)       // navegação
               .WithOne(pm => pm.Payment)          // navegação inversa
               .HasForeignKey<Payment>(p => p.PaymentMethodId); // FK escalar


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
