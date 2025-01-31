using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Exceptions.Favorites
{
    public class FavoriteNotFoundException : Exception
    {
        public FavoriteNotFoundException() { }
        public FavoriteNotFoundException(string message) : base(message) { }
        public FavoriteNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
