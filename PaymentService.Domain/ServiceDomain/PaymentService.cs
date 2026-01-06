using PaymentService.Domain.Agregates;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Interfaces;

namespace PaymentService.Domain.ServiceDomain
{
    /// <summary>
    /// Value object / helper para operações relacionadas a pagamentos.
    /// Foi refatorada para corrigir retornos, validar nulls e consertar a lógica dos cálculos.
    /// </summary>
    public class PaymentService : IPayment
    {
        private readonly Payment _paymentValue;

        public PaymentType Type => _paymentValue.Type;

        public PaymentService(Payment payment)
        {
            _paymentValue = payment ?? throw new ArgumentNullException(nameof(payment));
        }

        /// <summary>
        /// Calcula o valor aplicando primeiro o desconto e depois o imposto (se informado).
        /// Ex: amount = 100, discountPercent = 10, taxPercent = 5 => ((100 - 10%) + 5% sobre o valor com desconto).
        /// </summary>
        public Task<decimal> CalculatePaymentDiscount(decimal amount, decimal discountPercent, decimal taxPercent)
        {
            if (amount < 0) throw new ArgumentException("Amount não pode ser negativo.", nameof(amount));
            if (discountPercent < 0 || discountPercent > 100) throw new ArgumentException("discountPercent deve estar entre 0 e 100.", nameof(discountPercent));
            if (taxPercent < 0 || taxPercent > 100) throw new ArgumentException("taxPercent deve estar entre 0 e 100.", nameof(taxPercent));

            var discount = amount * (discountPercent / 100m);
            var afterDiscount = amount - discount;

            if (taxPercent == 0m)
                return Task.FromResult(afterDiscount);

            var tax = afterDiscount * (taxPercent / 100m);
            var final = afterDiscount + tax;
            return Task.FromResult(decimal.Round(final, 2));
        }

        /// <summary>
        /// Calcula o valor com imposto aplicado.
        /// </summary>
        public Task<decimal> CalculatePaymentTax(decimal amount, decimal taxPercent)
        {
            if (amount < 0) throw new ArgumentException("Amount não pode ser negativo.", nameof(amount));
            if (taxPercent < 0 || taxPercent > 100) throw new ArgumentException("taxPercent deve estar entre 0 e 100.", nameof(taxPercent));

            var tax = amount * (taxPercent / 100m);
            var final = amount + tax;
            return Task.FromResult(decimal.Round(final, 2));
        }

        /// <summary>
        /// Verifica se é possível cancelar a quantia informada do pagamento.
        /// Observação: a entidade Payment possui setters privados para estado/amount — 
        /// esta implementação apenas valida a operação e retorna se é possível.
        /// Se o domínio exigir alteração do Payment, deve expor um método de domínio (ex: payment.Cancel(amount)).
        /// </summary>
        public Task<bool> CancelPayment(Payment payment, decimal amount)
        {
            if (payment == null) return Task.FromResult(false);
            if (amount <= 0m) return Task.FromResult(false);
            if (payment.Amount <= 0m) return Task.FromResult(false);
            if (payment.Amount < amount) return Task.FromResult(false);

            // Não mutamos Payment aqui porque suas propriedades têm setters privados.
            // A ação de cancelamento deve ser feita através de um método da entidade Payment.
            return Task.FromResult(true);
        }

        /// <summary>
        /// Valida e cria um pagamento (validação leve aqui).
        /// Retorna true se os dados mínimos forem válidos.
        /// </summary>
        public Task<bool> CreatePayment(Payment payment)
        {
            if (payment == null) return Task.FromResult(false);
            if (payment.Amount <= 0m) return Task.FromResult(false);
            if (payment.FinalAmount.Value <= 0m) return Task.FromResult(false);

            // Lógica de criação persistente não fica aqui — este VO faz validações apenas.
            return Task.FromResult(true);
        }

        /// <summary>
        /// Valida detalhes específicos do método de pagamento.
        /// Para cartão de crédito, espera um CreditCardPayment e delega a validação.
        /// Para outros tipos, considera válido quando não há detalhes específicos.
        /// </summary>
        public Task<bool> ValidatePaymentDetails(object paymentDetails)
        {
            if (paymentDetails == null)
                return Task.FromResult(false);

            if (_paymentValue.Type == PaymentType.Credit)
            {
                if (paymentDetails is CreditCardPayment card)
                {
                    try
                    {
                        var valid = card.ValidationCreditCardPayment(card.Number, card.Holder, card.ExpirationDate, card.SecurityCode);
                        return Task.FromResult(valid);
                    }
                    catch
                    {
                        return Task.FromResult(false);
                    }
                }

                return Task.FromResult(false);
            }

            // Para débito/pix nenhum detalhe extra é obrigatório aqui
            return Task.FromResult(true);
        }

        /// <summary>
        /// Valida se o tipo informado bate com o tipo do pagamento encapsulado.
        /// </summary>
        public Task<bool> ValidatePaymentMethod(PaymentType type)
        {
            return Task.FromResult(_paymentValue.Type == type);
        }
       
    }
}