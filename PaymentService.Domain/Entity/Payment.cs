using PaymentService.Domain.ValueObjects;

namespace PaymentService.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public PaymentType Type { get; private set; }
        public Money OriginalAmount { get; private set; }
        public Discount Discount { get; private set; }
        public Installments Installments { get; private set; }
        public Money FinalAmount { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private readonly List<PaymentItem> _items = new();
        public IReadOnlyCollection<PaymentItem> Items => _items.AsReadOnly();

        // Construtor protegido para EF Core
        protected Payment() { }

        public Payment(PaymentType type, Money originalAmount, Discount discount, Installments installments)
        {
            Type = type;
            OriginalAmount = originalAmount;
            Discount = discount;
            Installments = installments;

            Calculate();
        }

        /// <summary>
        /// Aplica regras de negócio: desconto por tipo e acréscimo em crédito parcelado.
        /// </summary>
        private void Calculate()
        {
            // Aplica desconto
            var discounted = new Money(OriginalAmount.Value * Discount.AsFactor());

            _items.Clear();

            switch (Type)
            {
                case PaymentType.Credit:
                    var baseInstallment = new Money(discounted.Value / Installments.Count);

                    // Regra: se for crédito com 2 ou mais parcelas, acrescenta 2% em cada parcela
                    var installmentValue = Installments.HasCreditSurcharge
                        ? new Money(baseInstallment.Value * 1.02m)
                        : baseInstallment;

                    for (int i = 1; i <= Installments.Count; i++)
                        _items.Add(new PaymentItem(i, installmentValue));

                    FinalAmount = _items.Aggregate(new Money(0), (acc, it) => acc.Add(it.Amount));
                    break;

                case PaymentType.Debit:
                case PaymentType.Pix:
                    _items.Add(new PaymentItem(1, discounted));
                    FinalAmount = discounted;
                    break;

                default:
                    throw new InvalidOperationException("Tipo de pagamento inválido.");
            }

            FinalAmount = new Money(FinalAmount.Value); // arredondamento final
        }
    }
}