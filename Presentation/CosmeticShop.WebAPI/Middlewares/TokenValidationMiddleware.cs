using CosmeticShop.Domain.Repositories;
using CosmeticShop.Domain.Services;
using System.IdentityModel.Tokens.Jwt;

namespace CosmeticShop.WebAPI.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtTokenService _jwtTokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenValidationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The request delegate to pass control to the next middleware.</param>
        /// <param name="jwtTokenService">Service for interacting with JWT tokens.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="next"/> or <paramref name="jwtTokenService"/> is null.</exception>
        public TokenValidationMiddleware(RequestDelegate next, JwtTokenService jwtTokenService)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        }

        /// <summary>
        /// Validates the presence and validity of the JWT token for each request.
        /// </summary>
        /// <param name="context">The HTTP request context.</param>
        /// <param name="cancellationToken">A cancellation token to cancel long-running operations.</param>
        /// <returns>A Task representing the asynchronous middleware operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
        public async Task InvokeAsync(HttpContext context, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var jti = context.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                if (!string.IsNullOrEmpty(jti))
                {
                    var token = await _jwtTokenService.FindTokenByJti(jti, cancellationToken);

                    if (token == null || token.IsExpired())
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Token is invalid or expired.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
