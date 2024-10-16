using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CosmeticShop.DB.EF.Repositories
{
    public class OrderRepositoryEf : EfRepository<Order>, IOrderRepository
    {
        public OrderRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<IReadOnlyList<Order>> GetAll(CancellationToken cancellationToken)
        {
            var orders = await Entities.Include(o => o.Customer)
                                 .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.Product)
                                 .ToListAsync(cancellationToken);

            return orders;
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken)
        {
            return await Entities.Where(o => o.CustomerId == customerId).ToListAsync(cancellationToken);
        }
    }
}
