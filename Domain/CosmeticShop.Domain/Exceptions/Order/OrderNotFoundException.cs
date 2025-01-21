﻿namespace CosmeticShop.Domain.Exceptions.Order
{
    [Serializable]
    public class OrderNotFoundException : DomainException
    {
        public OrderNotFoundException()
        {
        }

        public OrderNotFoundException(string? message) : base(message)
        {
        }

        public OrderNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}