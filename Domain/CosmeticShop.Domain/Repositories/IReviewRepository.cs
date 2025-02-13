using CosmeticShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Repositories
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IReadOnlyList<Review>> GetAllNotApprovedReviews(CancellationToken cancellationToken);
        Task<IReadOnlyList<Review>> GetApprovedReviewsByProductId(Guid productId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Review>> GetReviewsByProductId(Guid productId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Review>> GetReviewsByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Review>> GetAllApprovedReviews(CancellationToken cancellationToken0);
    }
}
