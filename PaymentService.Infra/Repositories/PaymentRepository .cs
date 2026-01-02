using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;
using PaymentService.Infra.Data;

namespace PaymentService.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _db;

        public PaymentRepository(PaymentDbContext db)
        {
            _db = db;
        }

        public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Payments
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<IReadOnlyList<Payment>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Payments
                .Include(p => p.Items)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task AddAsync(Payment payment, CancellationToken ct = default)
        {
            await _db.Payments.AddAsync(payment, ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _db.SaveChangesAsync(ct);
        }
    }
}