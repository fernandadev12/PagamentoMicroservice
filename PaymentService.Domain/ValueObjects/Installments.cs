using System;

namespace PaymentService.Domain.ValueObjects
{
    public readonly struct Installments
    {
        public int Count { get; }

        public Installments(int count)
        {
            if (count <= 0) throw new ArgumentException("Parcelas devem ser >= 1.");
            Count = count;
        }

        /// <summary>
        /// Regra: se for crédito com 2 ou mais parcelas, aplica acréscimo de 2% por parcela.
        /// </summary>
        public bool HasCreditSurcharge => Count >= 2;
    }
}