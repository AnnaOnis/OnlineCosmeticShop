using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
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
    }
}
