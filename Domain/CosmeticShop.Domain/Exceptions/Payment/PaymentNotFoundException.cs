namespace CosmeticShop.Domain.Exceptions.Payment
{
    [Serializable]
    internal class PaymentNotFoundException : DomainException
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