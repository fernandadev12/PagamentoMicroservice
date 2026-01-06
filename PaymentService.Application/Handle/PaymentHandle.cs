using PaymentService.Application.Command;
using PaymentService.Application.DTO;
using PaymentService.Application.Querie;
using PaymentService.Application.Service;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;
using PaymentService.Domain.ValueObjects;

namespace PaymentService.Application.Handle
{
    public class PaymentHandle
    {
        private readonly IPaymentService _service;

        public PaymentHandle(IPaymentService service)
        {
            _service = service;
        }
        public async Task<PaymentResponseDTO> Handle(CreatePaymentCommand request, CancellationToken ct)
        {
            var r = request.Request;

            var type = (PaymentType)r.Type;
            var discount = new Discount(r.DiscountPercent);
            var installments = new Installments(r.Installments);
            var amount = new Money(r.Amount);

            if (type != PaymentType.Credit && installments.Count != 1)
                throw new ArgumentException("Somente crédito aceita múltiplas parcelas.");

            var payment = new Payment(type, amount.Value, amount, discount, installments);

            await _service.ProcessPayment(payment);
           
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

        public async Task<PaymentResponseDTO> Handle(UpdatePaymentCommand request, CancellationToken ct)
        {
            var r = request.Request;

            var type = (PaymentType)r.Type;
            var discount = new Discount(r.DiscountPercent);
            var installments = new Installments(r.Installments);
            var amount = new Money(r.Amount);

            var paymentUpdate = new Payment(type, amount.Value, amount, discount, installments);

            await _service.UpdateChangesAsync(paymentUpdate);
            var responsePayment = new PaymentResponseDTO
            {
                Id = paymentUpdate.Id,
                Type = (int)paymentUpdate.Type,
                OriginalAmount = paymentUpdate.OriginalAmount.Value,
                DiscountPercent = paymentUpdate.Discount.Percent,
                Installments = paymentUpdate.Installments.Count,
                FinalAmount = paymentUpdate.FinalAmount.Value,
                Items = paymentUpdate.Items.Select(i => new PaymentItemResp(i.Sequence, i.Amount.Value)).ToList()
            };

            return responsePayment;
        }

        public async Task<PaymentResponseDTO> Handle(CancelPaymentCommand request, CancellationToken ct)
        {
            var paymentID = request.Id;
            await _service.CancelPayment(paymentID);
            return new PaymentResponseDTO();
        }

        public async Task<PaymentResponseDTO> Handle(GetPaymentByIdQuery request, CancellationToken ct)
        {
            var paymentID = request.Id;
            var payment = await _service.GetByIdAsync(paymentID);

            var response = new PaymentResponseDTO
            {
                Id = payment.Id,
                Type = (int)payment.Type,
                OriginalAmount = payment.OriginalAmount.Value,
                DiscountPercent = payment.Discount.Percent,
                Installments = payment.Installments.Count,
                FinalAmount = payment.FinalAmount.Value,
                Items = payment.Items.Select(i => new PaymentItemResp(i.Sequence, i.Amount.Value)).ToList()
            };
            
            return payment.Id == Guid.Empty ? new PaymentResponseDTO() : response;
        }

        public async Task<IReadOnlyList<PaymentResponseDTO>> Handle(GetListPaymentsQuerie request, CancellationToken ct)
        {
            var payments = await _service.GetAllPayments();
            return payments.Select(p => new PaymentResponseDTO
            {
                Id = p.Id,
                Type = (int)p.Type,
                OriginalAmount = p.OriginalAmount.Value,
                DiscountPercent = p.Discount.Percent,
                Installments = p.Installments.Count,
                FinalAmount = p.FinalAmount.Value,
                Items = p.Items.Select(i => new PaymentItemResp(i.Sequence, i.Amount.Value)).ToList()
            }).ToList();
        }
    }
}