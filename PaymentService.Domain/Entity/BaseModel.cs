namespace PaymentService.Domain.Entity
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public TimeSpan DateModified { get; set; }
    }
}
