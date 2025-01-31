using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticShop.Domain.Entities;

namespace CosmeticShop.Domain.Repositories
{
    public interface IFavoriteRepository : IRepository<Favorite>
    {
        Task<IReadOnlyList<Favorite>> GetFavoritesByCustomerId(Guid customerId, CancellationToken cancellationToken);
        Task<(IReadOnlyList<Favorite>, int)> GetFavoritesByCustomerIdPaginations(Guid customerId, CancellationToken cancellationToken, int page = 1, int pageSize = 10);
        Task<IReadOnlyList<Favorite>> GetFavoritesByProductId(Guid productId, CancellationToken cancellationToken, int page = 1, int pageSize = 10);
        Task<bool> ExistsFavorite(Guid customerId, Guid productId, CancellationToken cancellationToken);
        Task<Favorite?> FindAsync(Guid customerId, Guid productId, CancellationToken cancellationToken);
    }
}
