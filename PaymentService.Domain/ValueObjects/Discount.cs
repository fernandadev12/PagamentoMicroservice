using System;

namespace PaymentService.Domain.ValueObjects
{
    public readonly struct Discount
    {
        public decimal Percent { get; }

        public Discount(decimal percent)
        {
            if (percent < 0 || percent > 100)
                throw new ArgumentException("Desconto deve estar entre 0 e 100%.");
            Percent = percent;
        }

        /// <summary>
        /// Retorna o fator multiplicador para aplicar desconto.
        /// Exemplo: 10% → 0.90
        /// </summary>
        public decimal AsFactor() => (100m - Percent) / 100m;
    }
}