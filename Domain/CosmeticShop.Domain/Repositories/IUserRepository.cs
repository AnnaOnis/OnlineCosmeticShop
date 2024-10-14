using CosmeticShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User?> FindByEmail(string email, CancellationToken cancellationToken);
        public Task<IReadOnlyList<User>> GetAllSorted(CancellationToken cancellationToken,
                                                           string? filter = null,
                                                           string? sortField = null,
                                                           string? sortOrder = null, 
                                                           int pageNumber = 1,
                                                           int pageSize = 10);
    }
}
