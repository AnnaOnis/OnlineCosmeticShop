using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Exceptions.Favorites;
using CosmeticShop.Domain.Exceptions.Customer;
using Microsoft.Extensions.Logging;
using CosmeticShop.Domain.Exceptions.Product;

namespace CosmeticShop.Domain.Services
{
    public class FavoriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FavoriteService> _logger;

        public FavoriteService(IUnitOfWork unitOfWork, ILogger<FavoriteService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Favorite> GetFavoriteAsync(Guid id, CancellationToken cancellationToken)
        {
            var favorite = await _unitOfWork.FavoriteRepository.GetById(id, cancellationToken);
            if (favorite == null)
            {
                throw new FavoriteNotFoundException();
            }
            return favorite;
        }

        public async Task<IReadOnlyList<Favorite>> GetFavoritesByCustomerIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var favorites = await _unitOfWork.FavoriteRepository.GetFavoritesByCustomerId(id, cancellationToken);
            return favorites;
        }

        public async Task<(IReadOnlyList<Favorite>, int)> GetPaginationsFavoritesByCustomerIdAsync(Guid id, CancellationToken cancellationToken, int page = 1, int pageSize = 10)
        {
            var (favorites, totalFavorites) = await _unitOfWork.FavoriteRepository.GetFavoritesByCustomerIdPaginations(id, cancellationToken, page, pageSize);
            return (favorites, totalFavorites);
        }

        public async Task<IReadOnlyList<Favorite>> GetFavoritesByProductIdAsync(Guid id, CancellationToken cancellationToken, int page = 1, int pageSize = 10)
        {
            var favorites = await _unitOfWork.FavoriteRepository.GetFavoritesByProductId(id, cancellationToken, page, pageSize);
            return favorites;
        }

        public async Task AddToFavoritesAsync(Guid customerId, Guid productId, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.CustomerRepository.GetById(customerId, cancellationToken);
            if (customer == null) throw new CustomerNotFoundException();

            var product = await _unitOfWork.ProductRepository.GetById(productId, cancellationToken);
            if (product == null) throw new ProductNotFoundException();

            var isExists = await _unitOfWork.FavoriteRepository.ExistsFavorite(customerId, productId, cancellationToken);
            if (isExists) throw new ProductAlreadyInFavoritesException();

            var favorite = new Favorite(customerId, productId);

            await _unitOfWork.FavoriteRepository.Add(favorite, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveFromFavoritesAsync(Guid customerId, Guid productId, CancellationToken cancellationToken)
        {
            var favorite = await _unitOfWork.FavoriteRepository.FindAsync(customerId, productId, cancellationToken);
            if(favorite == null) return;

            await _unitOfWork.FavoriteRepository.Delete(favorite.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
