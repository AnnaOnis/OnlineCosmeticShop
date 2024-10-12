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
    /// Represents a favorite product for a customer.
    /// </summary>
    public class Favorite
    {
        /// <summary>
        /// Gets or sets the unique identifier for the customer.
        /// </summary>
        public Guid CustomerId { get; init; }

        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public Guid ProductId { get; init; }

        /// <summary>
        /// Gets or sets the customer who marked the product as a favorite.
        /// </summary>
        public Customer Customer { get; set; } = null!;

        /// <summary>
        /// Gets or sets the product marked as a favorite by the customer.
        /// </summary>
        public Product Product { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="Favorite"/> class.
        /// </summary>
        /// <param name="customerId">The unique identifier for the customer.</param>
        /// <param name="productId">The unique identifier for the product.</param>
        public Favorite(Guid customerId, Guid productId)
        {
            CustomerId = customerId;
            ProductId = productId;
        }
    }
}
