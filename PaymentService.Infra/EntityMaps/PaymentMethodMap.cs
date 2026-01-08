using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain.Entity;

namespace PaymentService.Infra.EntityMaps
{
    public class PaymentMethodMap : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.ToTable("PaymentMethods"); 

            builder.HasKey(pm => pm.Id);
            builder.OwnsOne(pm => pm.creditCard, cc =>
            {
                cc.Property(c => c.Number).HasColumnName("CreditCardNumber");
            });

            // Pix
            builder.OwnsOne(pm => pm.pix);

            // Débito
            builder.OwnsOne(pm => pm.debitCard);
            builder.HasOne(pm => pm.Payment)           // navegação
                .WithOne(p => p.PaymentMethod)      // navegação inversa
                .HasForeignKey<PaymentMethod>(pm => pm.PaymentId); // FK escalar


        }
    }
}
