using CosmeticShop.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Services
{
    public class JwtTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(IUnitOfWork unitOfWork, ILogger<JwtTokenService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<JwtToken?> FindTokenByJti(string jti, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(jti, nameof(jti));

            return await _unitOfWork.JwtTokenRepository.FindTokenByJti(jti, cancellationToken);
        }

        public async Task AddTokenAsync(JwtToken userToken, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(userToken));

            var existedToken = await _unitOfWork.JwtTokenRepository.FindTokenByJti(userToken.Jti, cancellationToken);
            if (existedToken is null)
            {
                await _unitOfWork.JwtTokenRepository.Add(userToken, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> RemoveTokenByJtiAsync(string jti, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(jti, nameof(jti));

            var existedToken = await _unitOfWork.JwtTokenRepository.FindTokenByJti(jti, cancellationToken);
            if (existedToken != null)
            {
                await _unitOfWork.JwtTokenRepository.Delete(existedToken.Id, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return true;
            }

            return false;
        }
    }
}
