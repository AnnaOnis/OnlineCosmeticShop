using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.DB.EF.Repositories
{
    public class UserActionRepositoryEf : EfRepository<UserAction>, IUserActionRepository
    {
        public UserActionRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
