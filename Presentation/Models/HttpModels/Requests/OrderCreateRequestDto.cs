using CosmeticShop.Domain.Enumerations;
using CosmeticShop.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HttpModels.Requests
{
    public class OrderCreateRequestDto
    {
        [Required]
        public Guid CustomerId { get; init; }

        [Range(1, int.MaxValue, ErrorMessage = "Total quantity must be at least 1.")]
        public int TotalQuantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public ShippingMethod OrderShippingMethod { get; set; }

        [Required]
        public PaymentMethod OrderPaymentMethod { get; set; }

        [Required]
        public List<CartItemRequestDto> CartItems { get; set; }
    }
}
