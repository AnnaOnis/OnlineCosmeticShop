using CosmeticShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        public Task<Payment?> FindById(Guid Id, CancellationToken cancellationToken);
        Task<Payment?> GetPaymentByOrderId(Guid id, CancellationToken cancellation);
    }
}
