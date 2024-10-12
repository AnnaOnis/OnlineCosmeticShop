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
    }
}
