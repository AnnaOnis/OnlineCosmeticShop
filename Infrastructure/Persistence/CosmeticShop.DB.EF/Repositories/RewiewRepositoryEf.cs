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
    public class RewiewRepositoryEf : EfRepository<Review>, IRewiewRepository
    {
        public RewiewRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Review>> GetAllNotApprovedReviews(CancellationToken cancellationToken)
        {
            return await Entities
                .Where(e => e.IsApproved == false)
                .OrderByDescending(e => e.ReviewDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Review>> GetApprovedReviewsByProductId(Guid productId, CancellationToken cancellationToken)
        {
            return await Entities
                .Where(e => e.ProductId == productId && e.IsApproved == true)
                .OrderBy(e => e.ReviewDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Review>> GetByProductId(Guid productId, CancellationToken cancellationToken)
        {
            return await Entities
                .Where(e => e.ProductId == productId)
                .OrderBy(e => e.ReviewDate)
                .ToListAsync(cancellationToken);
        }
    }
}
