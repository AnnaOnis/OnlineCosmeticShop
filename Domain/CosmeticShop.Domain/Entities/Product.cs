using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Entities
{
    /// <summary>
    /// Represents a product in the cosmetics store.
    /// </summary>
    public class Product : IEntity
    {
        /// <summary>
        /// Gets the unique identifier for the product.
        /// </summary>
        [Key]
        public Guid Id { get; init; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.00.")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets the category ID to which the product belongs.
        /// </summary>
        [Required]
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the stock quantity of the product.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; }

        /// <summary>
        /// Gets or sets the rating of the product.
        /// </summary>
        [Range(0.0, 5.0, ErrorMessage = "Rating must be between 0.0 and 5.0.")]
        public double Rating { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer of the product.
        /// </summary>
        [StringLength(100, ErrorMessage = "Manufacturer name cannot exceed 100 characters.")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the date the product was added.
        /// </summary>
        [Required]
        public DateTime DateAdded { get; set; }

        /// <summary>
        /// Gets or sets the URL of the product image.
        /// </summary>
        public string ImageUrl { get; set; }

        // Navigation properties

        /// <summary>
        /// Gets or sets the category of the product.
        /// </summary>
        public Category Category { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of images related to the product.
        /// </summary>
        public ICollection<ProductImage> ProductImages { get; set; }

        /// <summary>
        /// Gets or sets the collection of reviews for the product.
        /// </summary>
        public ICollection<Review> Reviews { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <param name="name">The name of the product.</param>
        /// <param name="description">The description of the product.</param>
        /// <param name="price">The price of the product.</param>
        /// <param name="categoryId">The category ID of the product.</param>
        /// <param name="stockQuantity">The stock quantity of the product.</param>
        /// <param name="dateAdded">The date the product was added.</param>
        /// <exception cref="ArgumentException">Thrown when a required argument is invalid.</exception>
        public Product(string name, string description, decimal price, Guid categoryId, int stockQuantity, string manufacturer, string imageUrl = "")
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Product description is required.", nameof(description));
            if (price <= 0) throw new ArgumentOutOfRangeException("Price must be greater than 0.", nameof(price));
            if (stockQuantity < 0) throw new ArgumentOutOfRangeException("Stock quantity cannot be negative.", nameof(stockQuantity));
            if (string.IsNullOrWhiteSpace(manufacturer)) throw new ArgumentException("Product manufacturer is required.", nameof(manufacturer));

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            StockQuantity = stockQuantity;
            DateAdded = DateTime.Now;
            Manufacturer = manufacturer;
            Rating = 0;
            ImageUrl = imageUrl;

            ProductImages = new List<ProductImage>();
            Reviews = new List<Review>();
        }
    }
}
