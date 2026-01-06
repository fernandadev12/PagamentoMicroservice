namespace PaymentService.Application.DTO
{
    public record PaymentRequestDTO
    {
        public int Id { get; set; }            // Id
        public int Type { get; set; }            // 1=Credit, 2=Debit, 3=Pix
        public decimal Amount { get; set; }          // Valor original
        public decimal FinalAmount { get; set; }          // Valor final
        public decimal DiscountPercent { get; set; }  // Percentual de desconto
        public int Installments { get; set; }         // Número de parcelas
        public IReadOnlyList<PaymentItemResponse> Items { get; set; }

    }

    public record PaymentItemResponse(int Sequence, decimal Amount);
}

