using CosmeticShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerId(Guid userId, CancellationToken cancellationToken);
        Task<Order> GetOrderById(Guid id, CancellationToken cancellationToken);
        Task AddOrderItemsToOrder(Guid orderId, List<OrderItem> orderItems, CancellationToken cancellationToken);
        Task SaveOrderWithItemsAsync(Order order, List<CartItem> cartItems, CancellationToken cancellationToken);
    }
}
