using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticShop.Domain.Exceptions.Product;

namespace CosmeticShop.Domain.Entities
{
    /// <summary>
    /// Represents a shopping cart for a customer.
    /// </summary>
    public class Cart : IEntity
    {
        /// <summary>
        /// Gets the unique identifier for the cart.
        /// </summary>
        [Key]
        public Guid Id { get; init; }

        /// <summary>
        /// Gets the unique identifier of the customer who owns the cart.
        /// </summary>
        [Required]
        public Guid CustomerId { get; init; }

        /// <summary>
        /// Gets or sets the date when the cart was created.
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the total amount for the order.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Navigation property for the customer associated with the cart.
        /// </summary>
        public Customer Customer { get; set; } = null!;

        /// <summary>
        /// Navigation property for the items in the cart.
        /// </summary>
        public ICollection<CartItem> CartItems { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cart"/> class.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        public Cart(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            CreatedDate = DateTime.Now;
            CartItems = new List<CartItem>();
            TotalAmount = 0;
        }

        /// <summary>
        /// Adds an item to the cart. If the item already exists, the quantity is increased.
        /// </summary>
        /// <param name="productId">The ID of the product to add to the cart.</param>
        /// <param name="quantity">The quantity of the product to add. Must be greater than zero.</param>
        /// <returns>A completed task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the quantity is less than or equal to zero.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the CartItems collection is null.</exception>
        public Task AddItem(Guid productId, int quantity)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            if (CartItems == null) throw new InvalidOperationException(message: "Cart items is null");

            var existedCartItem = CartItems.SingleOrDefault(item => item.ProductId == productId);
            if (existedCartItem is null)
            {
                CartItems.Add(new CartItem(Id, productId, quantity));
            }
            else
            {
                existedCartItem.Quantity += quantity;
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates the quantity of an item in the user's cart.
        /// </summary>
        /// <param name="productId">The ID of the product to add to the cart.</param>
        /// <param name="quantity">The quantity of the product to add. Must be greater than zero.</param>
        /// <returns>A completed task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the quantity is less than or equal to zero.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the CartItems collection is null.</exception>
        /// <exception cref="ProductNotFoundException">Thrown when the Product not found in cart.</exception>
        public Task UpdateItemQuantity(Guid productId, int quantity)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            if (CartItems == null) throw new InvalidOperationException(message: "Cart items is null");

            var existedCartItem = CartItems.SingleOrDefault(item => item.ProductId == productId);
            if (existedCartItem is null)
            {
                throw new ProductNotFoundException("Product not found in cart");
            }
            else
            {
                existedCartItem.Quantity = quantity;
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Removes an item from the cart by its product ID.
        /// </summary>
        /// <param name="productId">The ID of the product to remove from the cart.</param>
        /// <returns>A completed task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the CartItems collection is null.</exception>
        /// <exception cref="ProductNotFoundException">Thrown when the Product not found in cart.</exception>
        public Task RemoveItem(Guid productId)
        {
            if (CartItems == null) throw new InvalidOperationException(message: "Cart items is null");

            var item = CartItems.SingleOrDefault(item => item.ProductId == productId);
            if (item is not null)
            {
                CartItems.Remove(item);
            }
            else
            {
                throw new ProductNotFoundException("Product not found in cart");
            }

            return Task.CompletedTask;
        }



        /// <summary>
        /// Clears all items from the cart.
        /// </summary>
        /// <returns>A completed task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the CartItems collection is null.</exception>
        public Task ClearItems()
        {
            if (CartItems == null) throw new InvalidOperationException(message: "Cart items is null");

            CartItems.Clear();

            return Task.CompletedTask;
        }

    }
}
