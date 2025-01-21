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
        /// Row version for optimistic concurrency.
        /// </summary>       
        [ConcurrencyCheck]
        public uint xmin { get; set; }// Поле для контроля версии (транзакция)

        /// <summary>
        /// Initializes a new instance of the <see cref="Cart"/> class.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        public Cart(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            CreatedDate = DateTime.UtcNow;
            CartItems = new List<CartItem>();
            TotalAmount = 0;
        }
    }
}
