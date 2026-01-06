using PaymentService.Domain.Entities;

namespace PaymentService.Application.Service
{
    public interface IPaymentService
    {
        Task<bool> ProcessPayment(Payment payment);
        Task<bool> RefundPayment(Payment payment);
        Task<IEnumerable<Payment>> GetAllPayments();
        Task<Payment?> GetByIdAsync(Guid id);
        Task<bool> CancelPayment(Guid id);
        Task<bool> UpdateChangesAsync(Payment payment);
        Task<bool> SaveChangesAsync(Payment payment);


    }
}
