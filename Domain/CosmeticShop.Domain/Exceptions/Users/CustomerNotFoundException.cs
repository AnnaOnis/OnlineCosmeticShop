﻿namespace CosmeticShop.Domain.Exceptions.Customer
{
    [Serializable]
    public class CustomerNotFoundException : DomainException
    {
        public CustomerNotFoundException()
        {
        }

        public CustomerNotFoundException(string? message) : base(message)
        {
        }

        public CustomerNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}