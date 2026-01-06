namespace PaymentService.Application.DTO
{
    public record PaymentResponseDTO
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public int Installments { get; set; }
        public decimal FinalAmount { get; set; }
        public IReadOnlyList<PaymentItemResp> Items { get; set; }
    }

    public record PaymentItemResp
    {

        public int Sequence { get; init; }
        public decimal Amount { get; init; }
        public decimal Value { get; }
        public PaymentItemResp(int sequence, decimal value)
        {
            Sequence = sequence;
            Value = value;
        }
    }
}
