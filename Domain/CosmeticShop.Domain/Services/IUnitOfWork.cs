using CosmeticShop.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Services
{
    public interface IUnitOfWork
    {
        ICartRepository CartRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IOrderRepository OrderRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IProductRepository ProductRepository { get; }
        IRewiewRepository RewiewRepository { get; }
        IUserActionRepository UserActionRepository { get; }
        IUserRepository UserRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
