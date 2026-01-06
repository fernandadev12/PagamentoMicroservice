using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;

namespace PaymentService.Domain.Interfaces
{
    public interface IPayment
    {
        Task<decimal> CalculatePaymentDiscount(decimal amount, decimal discountPercent, decimal taxPercent);
        Task<decimal> CalculatePaymentTax(decimal amount, decimal taxPercent);
        Task<bool> ValidatePaymentDetails(object paymentDetails);
        Task<bool> CreatePayment(Payment payment);
        Task<bool> CancelPayment(Payment payment, decimal amount);
        Task<bool> ValidatePaymentMethod(PaymentType type);
    }
}
