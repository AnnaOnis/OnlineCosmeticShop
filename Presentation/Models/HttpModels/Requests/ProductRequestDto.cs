using System.ComponentModel.DataAnnotations;

namespace HttpModels.Requests
{
    public class ProductRequestDto
    {
        [Required]
         [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

         [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.00.")]
        public decimal Price { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; }
        
        [Range(0.0, 5.0, ErrorMessage = "Rating must be between 0.0 and 5.0.")]
        public double Rating { get; set; }

        [StringLength(100, ErrorMessage = "Manufacturer name cannot exceed 100 characters.")]
        public string Manufacturer { get; set; }

        public string ImageUrl { get; set; }
    }
}