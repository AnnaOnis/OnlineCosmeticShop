using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Services
{
    /// <summary>
    /// Cart service that provides functionality to manage a customer's shopping cart.
    /// </summary>
    public class CartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CartService> _logger;

        public CartService(IUnitOfWork unitOfWork, ILogger<CartService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves the customer's cart. If no cart exists, returns an empty cart.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>The customer's cart.</returns>
        public async Task<Cart> GetCartByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            var cart =  await _unitOfWork.CartRepository.GetCartByCustomerId(customerId, cancellationToken);
            return cart ?? new Cart(customerId);
        }

        /// <summary>
        /// Adds an item to the customer's cart or updates the quantity if the item already exists.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="product">The product to add.</param>
        /// <param name="quantity">The quantity of the product to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the product is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the quantity is less than or equal to zero.</exception>
        public async Task AddItemToCartAsync(Guid customerId, Product product, int quantity, CancellationToken cancellationToken)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));

            var cart = await GetCartByCustomerIdAsync(customerId, cancellationToken);

            await cart.AddItem(product.Id, quantity);
            await _unitOfWork.CartRepository.Update(cart, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Removes an item from the customer's cart.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="productId">The ID of the product to remove.</param>
        public async Task RemoveItemFromCartAsync(Guid customerId, Guid productId, CancellationToken cancellationToken)
        {
            var cart = await GetCartByCustomerIdAsync(customerId, cancellationToken);
 
            await cart.RemoveItem(productId);
            await _unitOfWork.CartRepository.Update(cart, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the quantity of an item in the customer's cart.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="productId">The ID of the product to update.</param>
        /// <param name="quantity">The new quantity of the product.</param>
        /// /// <exception cref="ArgumentOutOfRangeException">Thrown when the quantity is less than or equal to zero.</exception>
        public async Task UpdateCartItemQuantityAsync(Guid customerId, Guid productId, int quantity, CancellationToken cancellationToken)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException("Quantity must be greater than zero");

            var cart = await GetCartByCustomerIdAsync(customerId, cancellationToken);

            await cart.UpdateItemQuantity(productId, quantity);
            await _unitOfWork.CartRepository.Update(cart, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Clears all items from the cart of the specified customer.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer whose cart will be cleared.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation of clearing the cart.</returns>
        public async Task ClearCartAsync(Guid customerId, CancellationToken cancellationToken)
        {
            var cart = await GetCartByCustomerIdAsync(customerId, cancellationToken);

            await cart.ClearItems();
            await _unitOfWork.CartRepository.Update(cart, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
