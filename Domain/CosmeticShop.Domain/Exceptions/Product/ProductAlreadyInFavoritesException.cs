namespace CosmeticShop.Domain.Exceptions.Product
{
    [Serializable]
    public class ProductAlreadyInFavoritesException : DomainException
    {
        public ProductAlreadyInFavoritesException()
        {
        }

        public ProductAlreadyInFavoritesException(string? message) : base(message)
        {
        }

        public ProductAlreadyInFavoritesException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}