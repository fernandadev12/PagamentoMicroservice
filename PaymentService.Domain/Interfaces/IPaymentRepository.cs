using PaymentService.Domain.Entities;

namespace PaymentService.Domain.Repositories
{
    public interface IPaymentRepository
    {
        Task <IEnumerable<Payment>> GetAllPayments();
        Task<Payment?> GetByIdAsync(Guid id);
        Task<bool> CancelPayment(Guid id);
        Task <bool> SaveChangesAsync(Payment pay);
        Task <bool> UpdateChangesAsync(Payment pay);
    }
}