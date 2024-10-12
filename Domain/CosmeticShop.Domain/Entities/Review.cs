using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Entities
{
    /// <summary>
    /// Represents a review left by a customer for a product.
    /// </summary>
    public class Review : IEntity
    {
        /// <summary>
        /// Gets the unique identifier of the review.
        /// </summary>
        [Key]
        public Guid Id { get; init; }

        /// <summary>
        /// Gets or sets the unique identifier of the product being reviewed.
        /// </summary>
        [Required]
        public Guid ProductId { get; init; }

        /// <summary>
        /// Gets or sets the unique identifier of the customer who left the review.
        /// </summary>
        [Required]
        public Guid CustomerId { get; init; }

        /// <summary>
        /// Gets or sets the text of the review.
        /// </summary>
        [StringLength(1000, ErrorMessage = "The review text cannot exceed 1000 characters.")]
        public string ReviewText { get; set; }

        /// <summary>
        /// Gets or sets the rating given by the customer.
        /// </summary>
        [Required]
        [Range(1, 5, ErrorMessage = "The rating must be between 1 and 5.")]
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the date when the review was left.
        /// </summary>
        public DateTime ReviewDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the review is approved.
        /// </summary>
        public bool IsApproved { get; set; }

        // Navigation properties

        /// <summary>
        /// Gets or sets the product associated with the review.
        /// </summary>
        public Product Product { get; set; } = null!;


        /// <summary>
        /// Gets or sets the customer who left the review.
        /// </summary>
        public Customer Customer { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="Review"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the review.</param>
        /// <param name="productId">The unique identifier of the product being reviewed.</param>
        /// <param name="customerId">The unique identifier of the customer who left the review.</param>
        /// <param name="reviewText">The text of the review.</param>
        /// <param name="rating">The rating given by the customer.</param>
        /// <exception cref="ArgumentNullException">Thrown when the review text is null or contains only a space.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the rating is out of the 1.0 to 5.0 range.</exception>
        public Review(Guid productId, Guid customerId, int rating, string reviewText="")
        {
            if (string.IsNullOrWhiteSpace(reviewText))
                throw new ArgumentNullException("Review text cannot be null or empty.", nameof(reviewText));

            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException("Rating must be between 1 and 5.", nameof(rating));

            Id = Guid.NewGuid();
            ProductId = productId;
            CustomerId = customerId; 
            Rating = rating;
            ReviewDate = DateTime.Now;
            IsApproved = false;
            ReviewText = reviewText;
        }
    }
}
