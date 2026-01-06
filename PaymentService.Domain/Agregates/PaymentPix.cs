using PaymentService.Domain.Entity;

namespace PaymentService.Domain.Agregates
{
    public class PaymentPix : BaseModel
    {
        public decimal Amount { get; private set; }
        public string NameOrder { get; private set; }
        public string KeyPix { get; private set; }
        public string QrCode { get; private set; }
        public string Bank { get; private set; }

        public PaymentPix(string nameOrder, decimal amount, string keyPix, string qrCode, string bank)
        {
            Amount = amount; 
            NameOrder = nameOrder; 
            KeyPix = keyPix; 
            QrCode = qrCode;
            Bank = bank;
        }
        public PaymentPix() { }

        public bool ValidationQrCode(string qrCode)
        {
            if (string.IsNullOrEmpty(qrCode))
                throw new ArgumentException("The qrCode cannot be null or empty.");

            return true;
        }

        public bool ValidationKeyPix(string keyPix)
        {
            if (string.IsNullOrEmpty(keyPix))
                throw new ArgumentException("The keyPix cannot be null or empty.");

            return true;
        }

    }
}
