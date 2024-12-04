using CosmeticShop.Domain.DTOs;
using CosmeticShop.Domain.Enumerations;

namespace CosmeticShop.Domain.Services
{
    /// <summary>
    /// Interface defining the contract for token generation services.
    /// </summary>
    public interface ITokenGenerationService
    {
        /// <summary>
        /// Generates a JWT token for a user.
        /// </summary>
        /// <param name="userId">Unique identifier of the user.</param>
        /// <param name="role">Optional user role. If not provided, a default role will be assigned.</param>
        /// <returns>A string containing the generated JWT token.</returns>
        JwtTokenDto GenerateToken(Guid userId, CancellationToken cancellationToken, RoleType? role = null);
        public string? ExtractJtiFromToken(string token, CancellationToken cancellationToken);
    }
}
