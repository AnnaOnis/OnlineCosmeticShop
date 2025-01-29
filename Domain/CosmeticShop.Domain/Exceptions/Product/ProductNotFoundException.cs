namespace CosmeticShop.Domain.Exceptions.Product
{
    [Serializable]
    public class ProductNotFoundException : DomainException
    {
        public ProductNotFoundException()
        {
        }

        public ProductNotFoundException(string? message) : base(message)
        {
        }

        public ProductNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}