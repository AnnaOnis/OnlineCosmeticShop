using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Entities
{
    /// <summary>
    /// Represents an image associated with a product.
    /// </summary>
    public class ProductImage : IEntity
    {
        /// <summary>
        /// Gets the unique identifier for the product image.
        /// </summary>
        [Key]
        public Guid Id { get; init; }

        /// <summary>
        /// Gets the identifier of the associated product.
        /// </summary>
        [Required]
        public Guid ProductId { get; init; }

        /// <summary>
        /// Gets or sets the URL of the image.
        /// </summary>
        [Required]
        [Url(ErrorMessage = "The ImageUrl field must be a valid URL.")]
        [StringLength(2048, ErrorMessage = "The ImageUrl cannot exceed 2048 characters.")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the associated product.
        /// </summary>
        public Product Product { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductImage"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the product image.</param>
        /// <param name="productId">The identifier of the associated product.</param>
        /// <param name="imageUrl">The URL of the product image.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="imageUrl"/> is null or empty.</exception>
        public ProductImage(Guid productId, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))throw new ArgumentNullException(nameof(imageUrl), "ImageUrl cannot be null or empty.");
   
            Id = Guid.NewGuid();
            ProductId = productId;
            ImageUrl = imageUrl;
        }
    }
}

