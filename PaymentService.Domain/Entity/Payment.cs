using PaymentService.Domain.Entity;
using PaymentService.Domain.Enums;
using PaymentService.Domain.ValueObjects;

namespace PaymentService.Domain.Entities
{
    public class Payment : BaseModel
    {
        public PaymentType Type { get; private set; }
        public Money OriginalAmount { get; private set; }
        public decimal Amount { get; private set; }
        public Discount Discount { get; private set; }
        public Installments Installments { get; private set; }
        public Money FinalAmount { get; private set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private readonly List<PaymentItem> _items = new();
        public IEnumerable<PaymentItem> Items => _items.AsReadOnly();
        public Guid PaymentMethodId { get; private set; }   // FK
        public PaymentMethod PaymentMethod { get; private set; } // Navegação


        // Construtor protegido para EF Core
        protected Payment() { }

        public Payment(decimal amount)
        {
            Amount = amount;
        }
        public Payment(PaymentType type, decimal amount, Money originalAmount, Discount discount, Installments installments)
        {
            Type = type;
            OriginalAmount = originalAmount;
            Discount = discount;
            Installments = installments;
            Amount = amount;
            Status = Status.Pending;
        }
    }
}