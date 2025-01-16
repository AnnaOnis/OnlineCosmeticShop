using CosmeticShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetCartByCustomerId(Guid customerId, CancellationToken cancellationToken);
        Task AddOrUpdateCartItemAsync(Guid customerId, Guid productId, int quantity, decimal productPrice, CancellationToken cancellationToken);

    }
}
