namespace CosmeticShop.Domain.Exceptions.Product
{
    [Serializable]
    internal class ProductNotFoundException : DomainException
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