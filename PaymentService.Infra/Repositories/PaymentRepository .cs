using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;
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

        public async Task<IEnumerable<Payment>> GetAllPayments()
        {
            return await _db.Payments
              .Include(p => p.Items)
              .OrderByDescending(p => p.CreatedAt)
              .ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(Guid id)
        {
            return await _db.Payments
                 .Include(p => p.Items)
                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> CancelPayment(Guid id)
        {
            var paymmentExist = await _db.Payments.FirstOrDefaultAsync(p => p.Id == id);
            if (paymmentExist == null)
            {
                throw new ArgumentException("Payment not found");
            }
            else
            {
                await _db.Payments.Where(p => p.Id == id)
                    .ExecuteUpdateAsync(p => p.SetProperty(pay => pay.Status, Status.Canceled));
            }

                return true;
        }

        public async Task<bool> SaveChangesAsync(Payment pay)
        {
            try
            {
                 _db.Payments.Add(pay);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException("Error save payment");
            }
            return true;
        }

        public async Task<bool> UpdateChangesAsync(Payment pay)
        {
            try
            {
                var entry = _db.Entry(pay);
                entry.State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ArgumentException("Error updating payment", ex);
            }
                return true;
        }
    }
}