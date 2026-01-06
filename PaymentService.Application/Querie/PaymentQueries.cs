using MediatR;
using PaymentService.Application.DTO;

namespace PaymentService.Application.Querie
{
    public class PaymentQueries { }
    
    public class GetPaymentByIdQuery : IRequest<PaymentResponseDTO>
    {
        private Guid id;

        public GetPaymentByIdQuery(Guid id)
        {
            this.id = id;
        }

        public Guid Id { get; set; }
    }
    public class GetListPaymentsQuerie : IRequest<PaymentResponseDTO>;
   
}
