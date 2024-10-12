namespace CosmeticShop.Domain.Exceptions.Users
{
    [Serializable]
    internal class UserNotFoundException : DomainException
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string? message) : base(message)
        {
        }

        public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}