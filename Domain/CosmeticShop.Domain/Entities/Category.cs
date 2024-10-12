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
    /// Represents a product category.
    /// </summary>
    public class Category : IEntity
    {
        /// <summary>
        /// Gets the unique identifier for the category.
        /// </summary>
        [Key]
        public Guid Id { get; init; }

        /// <summary>
        /// Gets or sets the unique identifier of the parent category.
        /// If the category has no parent, this value may be null.
        /// </summary>
        public Guid? ParentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string CategoryName { get; set; }

        // Navigation properties

        /// <summary>
        /// Gets or sets the parent category for this category.
        /// </summary>
        public Category ParentCategory { get; set; } = null!;

        /// <summary>
        /// Gets or sets the subcategories under this category.
        /// </summary>
        public ICollection<Category> SubCategories { get; set; }

        /// <summary>
        /// Gets or sets the products associated with this category.
        /// </summary>
        public ICollection<Product> Products { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="categoryName">The name of the category.</param>
        /// <param name="parentCategoryId">The unique identifier of the parent category, if any.</param>
        public Category(string categoryName, Guid? parentCategoryId = null)
        { 
            Id = Guid.NewGuid();
            CategoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
            ParentCategoryId = parentCategoryId;
            SubCategories = new List<Category>();
            Products = new List<Product>();
        }
    }
}
