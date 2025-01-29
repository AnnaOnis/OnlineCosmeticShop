namespace CosmeticShop.Domain.Exceptions.Payment
{
    [Serializable]
    public class InvalidPaymentMethodException : DomainException
    {
        public string PaymentMethod { get; }

        public InvalidPaymentMethodException(string paymentMethod, string message)
            : base(message)
        {
            PaymentMethod = paymentMethod;
        }

        public InvalidPaymentMethodException(string paymentMethod, string message, Exception innerException)
            : base(message, innerException)
        {
            PaymentMethod = paymentMethod;
        }
    }
}