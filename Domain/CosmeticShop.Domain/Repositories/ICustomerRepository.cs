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
        public Task<IReadOnlyList<User>> GetAllSorted(CancellationToken cancellationToken,
                                                   string? filter = null,
                                                   string? sortField = null,
                                                   string? sortOrder = null, 
                                                   int pageNumber = 1,
                                                   int pageSize = 10);
    }
}
