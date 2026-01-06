using PaymentService.Domain.Entity;

namespace PaymentService.Domain.Agregates
{
    public class CreditCardPayment : BaseModel
    {
        public string Number { get; private set; } = string.Empty;
        public string Holder { get; private set; } = string.Empty;
        public string ExpirationDate { get; private set; } = string.Empty;
        public string SecurityCode { get; private set; } = string.Empty;

        public CreditCardPayment(string number, string holder, string expirationDate, string securityCode)
        {
            Number = number;
            Holder = holder;
            ExpirationDate = expirationDate;
            SecurityCode = securityCode;
        }
        public CreditCardPayment() { }

        public bool ValidationCreditCardPayment(string number, string holder, string expirationDate, string securityCode) 
        {
            // Check if all parameters are not null or empty
            if (string.IsNullOrEmpty(number) || string.IsNullOrEmpty(holder) || string.IsNullOrEmpty(expirationDate) || string.IsNullOrEmpty(securityCode))
                throw new ArgumentException("All parameters cannot be null or empty.");

            // Check if the card number is valid using Luhn algorithm
            if (!IsValidCardNumber(number))
                throw new ArgumentException("The card number is invalid.");

            // Check if the expiration date is in the future
            if (!IsFutureDate(expirationDate))
                throw new ArgumentException("The expiration date must be in the future.");

           // Check if the security code is not empty
            if (string.IsNullOrEmpty(securityCode))
               throw new ArgumentException("The security code cannot be empty.");

            return true;
        }

        private bool IsValidCardNumber(string number)
        {
            int sum = 0;
            bool isSecondDigit = false;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(number[i].ToString());

                if (isSecondDigit)
                {
                    digit *= 2;

                    if (digit > 9)
                        digit -= 9;
                }

                sum += digit;
                isSecondDigit = !isSecondDigit;
            }

            return (sum % 10) == 0;
        }

        private bool IsFutureDate(string expirationDate)
        {
            DateTime today = DateTime.Today;
            DateTime expiration = DateTime.Parse(expirationDate);

            return expiration > today;
        }

    }
}
