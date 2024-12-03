using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.DTOs
{
    public record JwtTokenDto(string token, string jti, Guid UserId, DateTime Expiration);
}
