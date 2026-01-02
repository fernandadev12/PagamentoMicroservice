namespace PaymentService.Domain.ValueObjects
{
    public readonly struct Money
    {
        public decimal Value { get; }

        public Money(decimal value)
        {
            if (value < 0) throw new ArgumentException("Valor não pode ser negativo.");
            Value = decimal.Round(value, 2);
        }

        public Money Add(Money other) => new(Value + other.Value);
        public Money Subtract(Money other)
            => new(Value - other.Value < 0 ? throw new ArgumentException("Resultado negativo.") : Value - other.Value);

        public Money Percentage(decimal percent) => new(Value * percent / 100m);

        public override string ToString() => Value.ToString("F2");

        public static implicit operator decimal(Money m) => m.Value;
        public static implicit operator Money(decimal d) => new(d);
    }
}