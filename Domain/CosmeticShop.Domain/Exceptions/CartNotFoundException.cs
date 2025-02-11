namespace CosmeticShop.Domain.Exceptions
{
    [Serializable]
    public class CartNotFoundException : DomainException
    {
        public CartNotFoundException()
        {
        }

        public CartNotFoundException(string? message) : base(message)
        {
        }

        public CartNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}