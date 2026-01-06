using PaymentService.Domain.Entity;

namespace PaymentService.Domain.Agregates
{
    public class DebitPayment : BaseModel
    {
        public string Bank { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string ExpirationDate { get; set; } = string.Empty;
        public string SecurityCode { get; set; } = string.Empty;

        public DebitPayment() { }

        public bool ValidationDebitCardPayment()
        {
            // Check if all properties are not null or empty
            if (string.IsNullOrEmpty(Bank) || string.IsNullOrEmpty(CardNumber) || string.IsNullOrEmpty(ExpirationDate) || string.IsNullOrEmpty(SecurityCode))
                throw new ArgumentException("All properties cannot be null or empty.");

            // Check if the card number is valid using Luhn algorithm
            if (!IsValidCardNumber(CardNumber))
               throw new ArgumentException("The card number is invalid.");

            // Check if the expiration date is in the future
            if (!IsFutureDate(ExpirationDate))
               throw new ArgumentException("The expiration date must be in the future.");

            // Check if the security code is not empty
            if (string.IsNullOrEmpty(SecurityCode))
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
