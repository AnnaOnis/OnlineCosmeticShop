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
    /// Represents an item in a customer's order.
    /// </summary>
    public class OrderItem : IEntity
    {

        /// <summary>
        /// Unique identifier for the order item.
        /// </summary>
        [Key]
        public Guid Id { get; init; }

        /// <summary>
        /// Identifier of the order to which this item belongs.
        /// </summary>
        [Required]
        public Guid OrderId { get; init; }

        /// <summary>
        /// Identifier of the product associated with this order item.
        /// </summary>
        [Required]
        public Guid ProductId { get; init; }

        /// <summary>
        /// Quantity of the product in this order item.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        // Navigation properties

        /// <summary>
        /// The order to which this item belongs.
        /// </summary>
        public Order Order { get; set; } = null!;

        /// <summary>
        /// The product associated with this order item.
        /// </summary>
        public Product Product { get; set; } = null!;

        /// <summary>
        /// Parameterless constructor for Entity Framework.
        /// </summary>
        protected OrderItem() { } 

        /// <summary>
        /// Constructor for creating a new order item based on a cart item.
        /// </summary>
        /// <param name="item">The cart item.</param>
        /// <param name="orderId">The identifier of the order.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the quantity is less than 1.</exception>
        public OrderItem(CartItem item, Guid orderId)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "The cart item cannot be null.");

            if (item.Quantity < 1)
                throw new ArgumentOutOfRangeException(nameof(item.Quantity), "Quantity must be greater than 0.");

            Id = Guid.NewGuid();
            OrderId = orderId;
            ProductId = item.ProductId;
            Quantity = item.Quantity;
        }


    }
}
