using PaymentService.API.ViewModels;
using PaymentService.Application.DTO;

namespace Payment_Service.API.Mapper
{

    public static class PaymentMapper
    {
        public static PaymentViewModel ToViewModel(PaymentRequestDTO dto)
        {
            return new PaymentViewModel
            {

                PaymentType = dto.Type switch
                {
                    1 => "Crédito",
                    2 => "Débito",
                    3 => "Pix",
                    _ => "Desconhecido"
                },
                OriginalAmount = dto.Amount.ToString("C2"), // formato moeda
                DiscountApplied = $"{dto.DiscountPercent}%",
                Installments = dto.Installments,
                FinalAmount = dto.FinalAmount.ToString("C2"),
                Items = dto.Items.Select(i => new PaymentItemViewModel
                {
                    Sequence = i.Sequence,
                    Amount = i.Amount.ToString("C2")
                }).ToList()
            };
        }
    }
}
