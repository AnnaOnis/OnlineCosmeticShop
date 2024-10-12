using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Exceptions.Review
{
    [Serializable]
    public class ReviewNotFoundException : DomainException
    {
        public ReviewNotFoundException() { }
        public ReviewNotFoundException(string? message) : base(message) { }
        public ReviewNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
