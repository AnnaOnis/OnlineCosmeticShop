﻿using CosmeticShop.Domain.DTOs;
using CosmeticShop.Domain.Enumerations;
using CosmeticShop.Domain.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JwtTokenGenerator
{
    /// <summary>
    /// Service responsible for generating JWT tokens based on user information.
    /// Implements the <see cref="ITokenGenerationService"/> interface.
    /// </summary>
    public class JwtTokenGenerationService : ITokenGenerationService
    {
        private readonly JwtConfig _jwtConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenGenerationService"/> class with specified JWT configuration.
        /// </summary>
        /// <param name="jwtConfig">JWT configuration that contains settings such as the issuer, audience, signing key, and token lifetime.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="jwtConfig"/> is null.</exception>
        public JwtTokenGenerationService(JwtConfig jwtConfig)
        {
            _jwtConfig = jwtConfig ?? throw new ArgumentNullException(nameof(jwtConfig));
        }

        /// <summary>
        /// Generates a JWT token containing the user's ID and optional role information.
        /// </summary>
        /// <param name="userId">Unique identifier of the user.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <param name="role">Optional parameter specifying the role of the user. Defaults to "Customer" if not provided.</param>
        /// <returns>A <see cref="JwtTokenDto"/> containing the generated JWT token, JTI, user ID, and expiration time.</returns>
        /// <exception cref="InvalidOperationException">Thrown when token expiration is missing.</exception>
        public JwtTokenDto GenerateToken(Guid userId, CancellationToken cancellationToken, RoleType? role = null)
        {
            string userRole = role.HasValue ? role.Value.ToString() : "Customer";
            var jti = Guid.NewGuid().ToString();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, jti),
                    new Claim(ClaimTypes.Role, userRole)
                }),
                Expires = DateTime.UtcNow.Add(_jwtConfig.LifeTime),
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(_jwtConfig.SigningKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var tokenDto = new JwtTokenDto(tokenString, jti, userId, tokenDescriptor.Expires.Value);

            return tokenDto;
        }
       
        public string? ExtractJtiFromToken(string token, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(token);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Payload.Jti;
        }
    }
}
