namespace CosmeticShop.Domain.Exceptions.Customer
{
    [Serializable]
    internal class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException()
        {
        }

        public InvalidPasswordException(string? message) : base(message)
        {
        }

        public InvalidPasswordException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}