using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using CosmeticShop.Domain.Exceptions.Order;
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
            return await Entities.Include(o => o.OrderItems).Where(o => o.CustomerId == customerId).ToListAsync(cancellationToken);
        }

        public async Task<Order> GetOrderById(Guid id, CancellationToken cancellationToken)
        {
            var order = await Entities.Include(order => order.OrderItems)
                                        .ThenInclude(orderItem => orderItem.Product)
                                        .ThenInclude(product => product.Reviews)
                                      .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) throw new OrderNotFoundException("Order not found");
            return order;
        }

        public async Task AddOrderItemsToOrder(Guid orderId, List<OrderItem> orderItems, CancellationToken cancellationToken)
        {
            if (orderItems == null) throw new ArgumentNullException(nameof(orderItems));

            var or = await Entities.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == orderId);

            if (or == null) throw new OrderNotFoundException("Order not found");

            await _dbContext.Set<OrderItem>().AddRangeAsync(orderItems, cancellationToken);
            or.OrderItems = orderItems;
            //_dbContext.Entry(or).State = EntityState.Modified;
            _dbContext.Entry(or).Property(o => o.OrderItems).IsModified = true;
        }

        public async Task SaveOrderWithItemsAsync(Order order, List<CartItem> cartItems, CancellationToken cancellationToken)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                // Сохраняем заказ
                await _dbContext.Set<Order>().AddAsync(order, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken); // Сохраняем, чтобы получить order.Id

                // Сохраняем элементы заказа
                var orderItems = cartItems.Select(cartItem =>
                    new OrderItem(cartItem.ProductId, cartItem.Quantity, order.Id)).ToList();
                await _dbContext.Set<OrderItem>().AddRangeAsync(orderItems, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken); // Фиксируем изменения

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
