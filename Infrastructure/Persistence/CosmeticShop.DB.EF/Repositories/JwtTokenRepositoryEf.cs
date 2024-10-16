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
    public class JwtTokenRepositoryEf : EfRepository<JwtToken>, IJwtTokenRepository
    {
        public JwtTokenRepositoryEf(AppDbContext appDbContext) : base(appDbContext) { }

        public Task<JwtToken?> FindTokenByJti(string jti, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(jti, nameof(jti));

            return Entities.SingleOrDefaultAsync(e => e.Jti == jti, cancellationToken);
        }
    }
}
