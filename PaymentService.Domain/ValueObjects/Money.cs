namespace PaymentService.Domain.ValueObjects
{
    public record Money
    {
        public decimal Value { get; }

        public Money(decimal value)
        {
            if (value < 0) throw new ArgumentException("Valor não pode ser negativo.");
            Value = decimal.Round(value, 2);
        }

        public Money Add(Money money) => new(Value + money.Value);
        public Money Subtract(Money other)
            => new(Value - other.Value < 0 ? throw new ArgumentException("Resultado negativo.") : Value - other.Value);

        public Money Percentage(decimal percent) => new(Value * percent / 100m);

    }
}