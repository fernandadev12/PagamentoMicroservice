using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;

        public PaymentService(IPaymentRepository repo)
        {
            _repo = repo;
        }

        public Task<bool> CancelPayment(Guid id)
        {
          var payment = _repo.CancelPayment(id);
          return payment;
        }

        public async Task<IEnumerable<Payment>> GetAllPayments()
        {
            var listPayments = await _repo.GetAllPayments();
            return listPayments;
        }

        public Task<Payment?> GetByIdAsync(Guid id)
        {
            var payment = _repo.GetByIdAsync(id);
            return payment;
        }

        public async Task<bool> ProcessPayment(Payment pay)
        {
            try
            {
                pay.Status = Domain.Enums.Status.Processing;

                await _repo.SaveChangesAsync(pay);
            }
            catch (Exception)
            {
                throw new ArgumentException("Erro ao processar pagamento");
            }

            return true;
        }

        public async Task<bool> RefundPayment(Payment pay)
        {
            try
            {
                await _repo.CancelPayment(pay.Id);

            }
            catch (Exception)
            {
                throw new ArgumentException("Erro ao rejeitar pagamento");
            }
            return true;
        }

        public async Task<bool> SaveChangesAsync(Payment pay)
        {
            try
            {
                pay.Status = Domain.Enums.Status.Completed;

                await _repo.SaveChangesAsync(pay);
            }
            catch (Exception)
            {
                throw new ArgumentException("Erro ao processar pagamento");
            }

            return true;
        }

        public async Task<bool> UpdateChangesAsync(Payment pay)
        {
            try
            {
                await _repo.UpdateChangesAsync(pay);
            }
            catch (Exception)
            {
                throw new ArgumentException("Erro ao processar pagamento");
            }

            return true;
        }
    }
}
