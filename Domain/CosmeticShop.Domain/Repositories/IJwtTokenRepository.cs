using CosmeticShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Repositories
{
    public interface IJwtTokenRepository : IRepository<JwtToken>
    {
        Task<JwtToken?> FindTokenByJti(string token, CancellationToken cancellationToken);
    }
}
