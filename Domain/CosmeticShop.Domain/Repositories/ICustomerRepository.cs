using CosmeticShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        public Task<Customer?> FindByEmail(string email, CancellationToken cancellationToken);
    }
}
