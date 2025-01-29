namespace CosmeticShop.Domain.Exceptions.Category
{
    [Serializable]
    public class InvalidCategoryNameException : DomainException
    {
        public InvalidCategoryNameException()
        {
        }

        public InvalidCategoryNameException(string? message) : base(message)
        {
        }

        public InvalidCategoryNameException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}