using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Entities
{
    /// <summary>
    /// Represents a JWT token associated with a user.
    /// </summary>
    public class JwtToken : IEntity
    {
        /// <summary>
        /// Gets the unique identifier for the token record.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Gets the JWT token string.
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Gets the unique identifier for the token (JTI).
        /// </summary>
        public string Jti { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the user associated with the token.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the expiration time of the token.
        /// </summary>
        public DateTime Expiration { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtToken"/> class.
        /// </summary>
        /// <param name="token">The JWT token string.</param>
        /// <param name="jti">The unique token identifier (JTI).</param>
        /// <param name="userId">The identifier of the user associated with the token.</param>
        /// <param name="expiration">The expiration time of the token.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="token"/> or <paramref name="jti"/> is null.</exception>
        public JwtToken(string token, string jti, Guid userId, DateTime expiration)
        {
            Id = Guid.NewGuid();
            Token = token ?? throw new ArgumentNullException(nameof(token));
            Jti = jti ?? throw new ArgumentNullException(nameof(jti));
            UserId = userId;
            Expiration = expiration;
        }

        /// <summary>
        /// Determines if the token has expired.
        /// </summary>
        /// <returns>True if the token has expired; otherwise, false.</returns>
        public bool IsExpired() => DateTime.UtcNow >= Expiration;
    }

}
