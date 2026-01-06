using PaymentService.Domain.Agregates;
using PaymentService.Domain.Entities;

namespace PaymentService.Domain.Entity
{
    public class PaymentMethod : BaseModel
    {
        public string Name { get; set; }
        public CreditCardPayment creditCard { get; set; }
        public PaymentPix pix { get; set; }
        public DebitPayment debitCard { get; set; }
        public Payment PaymentId { get; set; }

    }
}
