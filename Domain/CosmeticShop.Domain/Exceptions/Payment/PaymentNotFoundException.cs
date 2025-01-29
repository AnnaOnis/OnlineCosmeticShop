namespace CosmeticShop.Domain.Exceptions.Payment
{
    [Serializable]
    public class PaymentNotFoundException : DomainException
    {
        public PaymentNotFoundException()
        {
        }

        public PaymentNotFoundException(string? message) : base(message)
        {
        }

        public PaymentNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}