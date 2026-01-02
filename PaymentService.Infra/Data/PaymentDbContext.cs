using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using PaymentService.Domain.ValueObjects;

namespace PaymentService.Infra.Data;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentItem> PaymentItems => Set<PaymentItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Type).IsRequired();
            b.Property(p => p.CreatedAt).IsRequired();

            b.OwnsOne(typeof(Money), "OriginalAmount", oa =>
            {
                oa.Property<decimal>("Value").HasColumnName("OriginalAmount").HasPrecision(18, 2);
            });

            b.OwnsOne(typeof(Money), "FinalAmount", fa =>
            {
                fa.Property<decimal>("Value").HasColumnName("FinalAmount").HasPrecision(18, 2);
            });

            b.OwnsOne(typeof(Discount), "Discount", d =>
            {
                d.Property<decimal>("Percent").HasColumnName("DiscountPercent").HasPrecision(5, 2);
            });

            b.OwnsOne(typeof(Installments), "Installments", i =>
            {
                i.Property<int>("Count").HasColumnName("InstallmentsCount");
            });

            b.HasMany(p => p.Items).WithOne().OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PaymentItem>(b =>
        {
            b.HasKey(i => i.Id);
            b.Property(i => i.Sequence).IsRequired();

            b.OwnsOne(typeof(Money), "Amount", a =>
            {
                a.Property<decimal>("Value").HasColumnName("Amount").HasPrecision(18, 2);
            });
        });
    }
}