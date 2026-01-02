using MediatR;
using PaymentService.Application.DTO;
using PaymentService.Domain.ValueObjects;
using PaymentService.Infra.Data;

namespace PaymentService.Application.Commands;

public record CreatePaymentCommand(PaymentRequestDTO Request) : IRequest<PaymentResponseDTO>;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, PaymentResponseDTO>
{
    private readonly PaymentDbContext _db;

    public CreatePaymentHandler(PaymentDbContext db) => _db = db;

    public async Task<PaymentResponseDTO> Handle(CreatePaymentCommand request, CancellationToken ct)
    {
        var r = request.Request;

        var type = (PaymentType)r.Type;
        var discount = new Discount(r.DiscountPercent);
        var installments = new Installments(r.Installments);
        var amount = new Money(r.Amount);

        if (type != PaymentType.Credit && installments.Count != 1)
            throw new ArgumentException("Somente crédito aceita múltiplas parcelas.");

        // Corrija o tipo para PaymentService.Domain.Entities.Payment
        var payment = new PaymentService.Domain.Entities.Payment(type, amount, discount, installments);

        await _db.Payments.AddAsync(payment);
        
        await _db.SaveChangesAsync(ct);
        var responsePayment = new PaymentResponseDTO
        {
            Id = payment.Id,
            Type = (int)payment.Type,
            OriginalAmount = payment.OriginalAmount.Value,
            DiscountPercent = payment.Discount.Percent,
            Installments = payment.Installments.Count,
            FinalAmount = payment.FinalAmount.Value,
            Items = payment.Items.Select(i => new PaymentItemResp(i.Sequence, i.Amount.Value)).ToList()
        };

        return responsePayment;
    }
}
