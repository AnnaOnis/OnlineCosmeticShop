﻿namespace CosmeticShop.Domain.Exceptions.Customer
{
    [Serializable]
    public class InvalidPasswordException : DomainException
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