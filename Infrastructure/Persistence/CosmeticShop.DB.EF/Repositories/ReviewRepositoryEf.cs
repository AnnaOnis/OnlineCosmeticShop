using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.DB.EF.Repositories
{
    public class ReviewRepositoryEf : EfRepository<Review>, IReviewRepository
    {
        public ReviewRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Review>> GetAllNotApprovedReviews(CancellationToken cancellationToken)
        {
            return await Entities
                .Include(e => e.Customer)
                .Where(e => e.IsApproved == false)
                .OrderBy(e => e.ReviewDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Review>> GetApprovedReviewsByProductId(Guid productId, CancellationToken cancellationToken)
        {
            return await Entities
                .Include(e => e.Customer)
                .Where(e => e.ProductId == productId && e.IsApproved == true)
                .OrderByDescending(e => e.ReviewDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Review>> GetReviewsByProductId(Guid productId, CancellationToken cancellationToken)
        {
            return await Entities
                .Where(e => e.ProductId == productId)
                .OrderByDescending(e => e.ReviewDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Review>> GetReviewsByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return await Entities
                .Where(r => r.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }
    }
}
