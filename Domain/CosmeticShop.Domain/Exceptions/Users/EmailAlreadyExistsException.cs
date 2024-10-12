namespace CosmeticShop.Domain.Exceptions.Customer
{
    [Serializable]
    internal class EmailAlreadyExistsException : DomainException
    {
        public EmailAlreadyExistsException()
        {
        }

        public EmailAlreadyExistsException(string? message) : base(message)
        {
        }

        public EmailAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}