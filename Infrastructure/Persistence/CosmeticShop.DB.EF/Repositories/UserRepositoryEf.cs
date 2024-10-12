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
    public class UserRepositoryEf : EfRepository<User>, IUserRepository
    {
        public UserRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<User?> FindByEmail(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException($"\"{nameof(email)}\" cannot be empty or contain only a space.", nameof(email));

            return Entities.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
        }
    }
}
