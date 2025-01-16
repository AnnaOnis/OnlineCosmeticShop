using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Exceptions.Product;
using CosmeticShop.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
            if (cart == null)
            {
                cart = new Cart(customerId);
                await _unitOfWork.CartRepository.Add(cart, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            return cart;
        }

        /// <summary>
        /// Adds an item to the customer's cart or updates the quantity if the item already exists.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="productId">The ID of the product to add.</param>
        /// <param name="quantity">The quantity of the product to add.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the quantity is less than or equal to zero.</exception>
        /// <exception cref="ProductNotFoundException">Throw when product not found.</exception>
        public async Task AddItemToCartAsync(Guid customerId, Guid productId, int quantity, CancellationToken cancellationToken)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));

            var product = await _unitOfWork.ProductRepository.GetById(productId, cancellationToken);
            if (product == null)
            {
                throw new ProductNotFoundException("Product not found.");
            }

            await _unitOfWork.CartRepository.AddOrUpdateCartItemAsync(customerId, product.Id, quantity, product.Price, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Removes an item from the customer's cart.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="productId">The ID of the product to remove.</param>
        /// <exception cref="ProductNotFoundException">Throw when product not found.</exception>
        public async Task RemoveItemFromCartAsync(Guid customerId, Guid productId, CancellationToken cancellationToken)
        {
            var cart = await GetCartByCustomerIdAsync(customerId, cancellationToken);

            var item = cart.CartItems.FirstOrDefault(item => item.ProductId == productId);
            if (item == null)
            {
                throw new ProductNotFoundException("Product not found in cart.");
            }

            cart.CartItems.Remove(item);
            cart.TotalAmount = cart.CartItems.Sum(item => item.ProductPrice * item.Quantity);
            await _unitOfWork.CartRepository.Update(cart, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the quantity of an item in the customer's cart.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="productId">The ID of the product to update.</param>
        /// <param name="quantity">The new quantity of the product.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the quantity is less than or equal to zero.</exception>
        /// <exception cref="ProductNotFoundException">Throw when product not found.</exception>
        public async Task UpdateCartItemQuantityAsync(Guid customerId, Guid productId, int quantity, CancellationToken cancellationToken)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException("Quantity must be greater than zero");

            var cart = await GetCartByCustomerIdAsync(customerId, cancellationToken);
            
            var item = cart.CartItems.FirstOrDefault(item => item.ProductId == productId);
            if (item == null)
            {
                throw new ProductNotFoundException("Product not found in cart.");
            }

            item.Quantity = quantity;
            cart.TotalAmount = cart.CartItems.Sum(item => item.ProductPrice * item.Quantity);
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

            cart.CartItems.Clear();
            cart.TotalAmount = cart.CartItems.Sum(item => item.ProductPrice * item.Quantity);
            await _unitOfWork.CartRepository.Update(cart, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
