using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;
using PaymentService.Domain.ValueObjects;

namespace PaymentService.Infra.Repositories
{
    public class PaymentVO
    {
        private readonly Payment _payment;
        private readonly string cardNumber;
        private readonly string expirationDate;
        private readonly string securityCode;
        private readonly List<PaymentItem> _items = new();
        public PaymentType Type => _payment.Type;
        public PaymentVO(Payment payment)
        {
            _payment = payment;
            Calculate();
        }

        public PaymentVO(Payment payment, string cardNumber, string expirationDate, string securityCode)
        {
            payment = _payment ?? throw new ArgumentNullException(nameof(payment));
            this.cardNumber = cardNumber;
            this.expirationDate = expirationDate;
            this.securityCode = securityCode;
        }
        /// <summary>
        /// Aplica regras de negócio: desconto por tipo e acréscimo em crédito parcelado.
        /// </summary>
        private void Calculate()
        {
            // Aplica desconto
            var discounted = new Money(_payment.OriginalAmount.Value * _payment.Discount.AsFactor());

            _items.Clear();

            switch (Type)
            {
                case PaymentType.Credit:
                    var baseInstallment = new Money(discounted.Value / _payment.Installments.Count);

                    // Regra: se for crédito com 2 ou mais parcelas, acrescenta 2% em cada parcela
                    var installmentValue = _payment.Installments.HasCreditSurcharge
                        ? new Money(baseInstallment.Value * 1.02m)
                        : baseInstallment;

                    for (int i = 1; i <= _payment.Installments.Count; i++)
                    {
                        _items.Add(new PaymentItem(i, installmentValue));
                    }

                    // _payment.FinalAmount.Value = installmentValue;
                    break;  

                case PaymentType.Debit:
                case PaymentType.Pix:
                    _items.Add(new PaymentItem(1, discounted));
                   // _payment.FinalAmount.Value = new Money(discounted);
                    break;

                default:
                    throw new InvalidOperationException("Tipo de pagamento inválido.");
            }

            var FinalAmount = new Money(_payment.FinalAmount.Value); // arredondamento final
        }


        public bool ValidationCreditCardPayment(string number, string holder, string expirationDate, string securityCode)
        {
            // Check if all parameters are not null or empty
            if (string.IsNullOrEmpty(number) || string.IsNullOrEmpty(holder) || string.IsNullOrEmpty(expirationDate) || string.IsNullOrEmpty(securityCode))
                throw new ArgumentException("All parameters cannot be null or empty.");

            // Check if the card number is valid using Luhn algorithm
            if (!IsValidCardNumber(number))
                throw new ArgumentException("The card number is invalid.");

            // Check if the expiration date is in the future
            if (!IsFutureDate(expirationDate))
                throw new ArgumentException("The expiration date must be in the future.");

            // Check if the security code is not empty
            if (string.IsNullOrEmpty(securityCode))
                throw new ArgumentException("The security code cannot be empty.");

            return true;
        }

        private bool IsValidCardNumber(string number)
        {
            int sum = 0;
            bool isSecondDigit = false;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(number[i].ToString());

                if (isSecondDigit)
                {
                    digit *= 2;

                    if (digit > 9)
                        digit -= 9;
                }

                sum += digit;
                isSecondDigit = !isSecondDigit;
            }

            return (sum % 10) == 0;
        }

        private bool IsFutureDate(string expirationDate)
        {
            DateTime today = DateTime.Today;
            DateTime expiration = DateTime.Parse(expirationDate);

            return expiration > today;
        }
    }
}