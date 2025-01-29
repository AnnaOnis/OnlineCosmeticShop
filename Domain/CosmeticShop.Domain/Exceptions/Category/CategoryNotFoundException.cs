namespace CosmeticShop.Domain.Exceptions.Category
{
    [Serializable]
    public class CategoryNotFoundException : DomainException
    {
        public CategoryNotFoundException()
        {
        }

        public CategoryNotFoundException(string? message) : base(message)
        {
        }

        public CategoryNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}