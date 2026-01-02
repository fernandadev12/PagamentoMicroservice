using PaymentService.Domain.Entities;

namespace PaymentService.Domain.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<Payment>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(Payment payment, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}