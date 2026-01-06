using PaymentService.Domain.Enums;

namespace PaymentService.Domain.Agregates
{
    public class PaymentOrder
    {
        public Guid Id { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentType PaymentMethod { get; private set; }
        public BankAccount BankAccount { get; private set; }
        public DateTime Date { get; private set; }
        public Status Status { get; private set; }

        public PaymentOrder(Guid id, decimal amount, PaymentType paymentMethod, BankAccount bankAccount)
        {
            Id = id;
            Amount = amount;
            PaymentMethod = paymentMethod;
            BankAccount = bankAccount;
            Date = DateTime.Now;
            Status = Status.Pending;
        }

        public void UpdateStatus(Status status)
        {
            Status = status;
        }
    }

    public class BankAccount
    {
        public Guid Id { get; private set; }
        public string Number { get; private set; }
        public string Agency { get; private set; }
        public string Bank { get; private set; }

        public BankAccount(string number, string agency, string bank)
        {
            Id = Guid.NewGuid();
            Number = number;
            Agency = agency;
            Bank = bank;
        }
    }
}
