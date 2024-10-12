using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CosmeticShop.DB.EF.Repositories
{
    public class CategoryRepositoryEf : EfRepository<Category>, ICategoryRepository
    {
        public CategoryRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException($"\"{nameof(name)}\" cannot be empty or contain only a space.", nameof(name));

            var category = await Entities
                        .SingleOrDefaultAsync(c => c.CategoryName == name, cancellationToken);

            return category != null;
        }
    }
}
