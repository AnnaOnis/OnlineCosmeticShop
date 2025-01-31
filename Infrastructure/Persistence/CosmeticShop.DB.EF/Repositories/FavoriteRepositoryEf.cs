using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CosmeticShop.DB.EF.Repositories
{
    public class FavoriteRepositoryEf : EfRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepositoryEf(AppDbContext context) : base(context) { }
        public async Task<bool> ExistsFavorite(Guid customerId, Guid productId, CancellationToken cancellationToken)
        {
            return await Entities.AnyAsync(f => f.CustomerId == customerId && f.ProductId == productId);
        }

        public async Task<Favorite?> FindAsync(Guid customerId, Guid productId, CancellationToken cancellationToken)
        {
            return await Entities.FirstOrDefaultAsync(f => f.CustomerId == customerId && f.ProductId == productId);
        }

        public async Task<IReadOnlyList<Favorite>> GetFavoritesByCustomerId(Guid customerId, CancellationToken cancellationToken)
        {
            return await Entities
                            .Where(f => f.CustomerId == customerId)
                            .Include(f => f.Product) // Загружаем связанный продукт
                            .OrderByDescending(f => f.DateAdded)
                            .ToListAsync();
        }

        public async Task<(IReadOnlyList<Favorite>, int)> GetFavoritesByCustomerIdPaginations(Guid customerId, CancellationToken cancellationToken, int page = 1, int pageSize = 12)
        {
            IQueryable<Favorite> query = Entities;

            query = query.Where(q => q.CustomerId == customerId);

            int totalFavorites = await query.CountAsync();

            // Применение пагинации
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var favorites = await query.Include(q => q.Product).ToListAsync(cancellationToken);

            return (favorites, totalFavorites);
        }

        public async Task<IReadOnlyList<Favorite>> GetFavoritesByProductId(Guid productId, CancellationToken cancellationToken, int page = 1, int pageSize = 10)
        {
            return await Entities
                            .Where(f => f.CustomerId == productId)
                            .Include(f => f.Product) // Загружаем связанный продукт
                            .OrderByDescending(f => f.DateAdded)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
        }
    }
}
