namespace CosmeticShop.Domain.Exceptions.Review
{
    [Serializable]
    public class ReviewAlreadyApprovedException : DomainException
    {
        public ReviewAlreadyApprovedException()
        {
        }

        public ReviewAlreadyApprovedException(string? message) : base(message)
        {
        }

        public ReviewAlreadyApprovedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}