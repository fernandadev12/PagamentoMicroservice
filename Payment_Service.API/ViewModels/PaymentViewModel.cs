namespace PaymentService.API.ViewModels
{

    public class PaymentViewModel
    {
        public Guid Id { get; set; }
        public string PaymentType { get; set; } = string.Empty; // "Crédito", "Débito", "Pix"
        public string OriginalAmount { get; set; } = string.Empty; // formatado em R$
        public string DiscountApplied { get; set; } = string.Empty; // ex: "10%"
        public int Installments { get; set; }
        public string FinalAmount { get; set; } = string.Empty; // formatado em R$
        public List<PaymentItemViewModel> Items { get; set; } = new();
    }

    public class PaymentItemViewModel
    {
        /// <summary>
        /// Número da parcela (1, 2, 3...)
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Valor da parcela formatado em moeda (ex: R$ 306,00)
        /// </summary>
        public string Amount { get; set; } = string.Empty;

    }
}

