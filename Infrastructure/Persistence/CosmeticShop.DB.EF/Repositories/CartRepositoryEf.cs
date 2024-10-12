using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CosmeticShop.DB.EF.Repositories
{
    public class CartRepositoryEf : EfRepository<Cart>, ICartRepository
    {
        public CartRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<Cart> GetCartByCustomerId(Guid customerId, CancellationToken cancellationToken)
        {
            return Entities.Include(cart => cart.CartItems)
                           .SingleAsync(e => e.CustomerId == customerId, cancellationToken);
        }
    }
}
