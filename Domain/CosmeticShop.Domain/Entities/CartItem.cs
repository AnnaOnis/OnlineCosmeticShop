using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Entities
{
    /// <summary>
    /// Represents an item in the shopping cart.
    /// </summary>
    public class CartItem : IEntity
    {
        /// <summary>
        /// Unique identifier for the cart item.
        /// </summary>
        [Key]
        public Guid Id { get; init; }

        /// <summary>
        /// Identifier for the related shopping cart.
        /// </summary>
        [Required]
        public Guid CartId { get; init; }

        /// <summary>
        /// Identifier for the related product.
        /// </summary>
        [Required]
        public Guid ProductId { get; init; }

        /// <summary>
        /// Quantity of the product in the cart.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // Navigation properties

        /// <summary>
        /// The cart that this item belongs to.
        /// </summary>
        public Cart Cart { get; set; } = null!;

        /// <summary>
        /// The product associated with this cart item.
        /// </summary>
        public Product Product { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartItem"/> class with required properties.
        /// </summary>
        /// <param name="cartId">The ID of the shopping cart.</param>
        /// <param name="productId">The ID of the product.</param>
        /// <param name="quantity">The quantity of the product.</param>
        public CartItem(Guid cartId, Guid productId, int quantity)
        {
            
            Id = Guid.NewGuid();
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity > 0 ? quantity : throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least 1.");
        }
    }
}
