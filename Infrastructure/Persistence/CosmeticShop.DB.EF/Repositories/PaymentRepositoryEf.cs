using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.DB.EF.Repositories
{
    public class PaymentRepositoryEf : EfRepository<Payment>, IPaymentRepository
    {
        public PaymentRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Payment?> FindById(Guid Id, CancellationToken cancellationToken)
        {
            return await Entities.SingleOrDefaultAsync(e => e.Id == Id, cancellationToken);
        }

        public override async Task<Payment> GetById(Guid Id, CancellationToken cancellationToken)
        {
            return await Entities
                .Include(e => e.Order)
                .Include(e => e.Customer)
                .FirstAsync(x => x.Id == Id, cancellationToken);
        }

        public async Task<Payment?> GetPaymentByOrderId(Guid id, CancellationToken cancellation)
        {
            return await Entities.SingleOrDefaultAsync(e => e.OrderId == id, cancellation);
        }
    }
}
