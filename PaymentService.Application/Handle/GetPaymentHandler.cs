using MediatR;
using PaymentService.Application.DTO;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Payments
{
    // Query para buscar pagamento por ID
    public record GetPaymentByIdQuery(Guid Id) : IRequest<PaymentResponseDTO?>;

    public class GetPaymentByIdHandler : IRequestHandler<GetPaymentByIdQuery, PaymentResponseDTO?>
    {
        private readonly IPaymentRepository _repository;

        public GetPaymentByIdHandler(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaymentResponseDTO?> Handle(GetPaymentByIdQuery request, CancellationToken ct)
        {
            var payment = await _repository.GetByIdAsync(request.Id, ct);
            return payment is null ? null : MapToDto(payment);
        }

        private static PaymentResponseDTO MapToDto(Payment payment)
        {
            return new PaymentResponseDTO
            {
                Id = payment.Id,
                Type = (int)payment.Type,
                OriginalAmount = payment.OriginalAmount.Value,
                DiscountPercent = payment.Discount.Percent,
                Installments = payment.Installments.Count,
                FinalAmount = payment.FinalAmount.Value,
                Items = (IReadOnlyList<PaymentItemResp>)payment.Items
                .Select(i => new PaymentItemResponse(i.Sequence, i.Amount.Value))
                .ToList()
            };
        }
    }

    // Query para listar todos os pagamentos
    public record GetPaymentsQuery() : IRequest<IReadOnlyList<PaymentResponseDTO>>;

    public class GetPaymentsHandler : IRequestHandler<GetPaymentsQuery, IReadOnlyList<PaymentResponseDTO>>
    {
        private readonly IPaymentRepository _repository;

        public GetPaymentsHandler(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<PaymentResponseDTO>> Handle(GetPaymentsQuery request, CancellationToken ct)
        {
            var payments = await _repository.GetAllAsync(ct);
            return payments.Select(MapToDto).ToList();
        }

        private static PaymentResponseDTO MapToDto(Payment payment)
        {
            return new PaymentResponseDTO
            {
                Id = payment.Id,
                Type = (int)payment.Type,
                OriginalAmount = payment.OriginalAmount.Value,
                DiscountPercent = payment.Discount.Percent,
                Installments = payment.Installments.Count,
                FinalAmount = payment.FinalAmount.Value,
                Items = (IReadOnlyList<PaymentItemResp>)payment.Items
                    .Select(i => new PaymentItemResponse(i.Sequence, i.Amount.Value))
                    .ToList()
            };
        }
    }
}