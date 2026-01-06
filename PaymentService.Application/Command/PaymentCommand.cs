using MediatR;
using PaymentService.Application.DTO;

namespace PaymentService.Application.Command
{
    public class PaymentCommand { }

    public class CreatePaymentCommand(PaymentRequestDTO Request) : IRequest<PaymentResponseDTO>
    {
        public PaymentRequestDTO Request { get; set; }
    }

    public class UpdatePaymentCommand : IRequest<PaymentResponseDTO>
    {
        public PaymentRequestDTO Request { get; set; }
    }

    public class CancelPaymentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
