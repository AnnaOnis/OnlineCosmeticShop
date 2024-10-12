using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CosmeticShop.DB.EF.Repositories
{
    public class CustomerRepositoryEf : EfRepository<Customer>, ICustomerRepository
    {
        public CustomerRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<Customer?> FindByEmail(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException($"\"{nameof(email)}\" cannot be empty or contain only a space.", nameof(email));

            return Entities.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
        }
    }
}
