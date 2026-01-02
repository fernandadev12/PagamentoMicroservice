using PaymentService.Domain.ValueObjects;
using System;

namespace PaymentService.Domain.Entities
{
    public class PaymentItem
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public int Sequence { get; private set; }
        public Money Amount { get; private set; }

        protected PaymentItem() { }

        public PaymentItem(int sequence, Money amount)
        {
            if (sequence <= 0) throw new ArgumentException("Sequência deve ser >= 1.");
            Sequence = sequence;
            Amount = amount;
        }
    }
}