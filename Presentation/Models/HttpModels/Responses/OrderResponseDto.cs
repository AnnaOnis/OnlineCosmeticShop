using CosmeticShop.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpModels.Responses
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }

        [Required]
        public Guid CustomerId { get; init; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Total quantity must be at least 1.")]
        public int TotalQuantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(100)]
        public ShippingMethod OrderShippingMethod { get; set; }

        [Required]
        [MaxLength(100)]
        public PaymentMethod OrderPaymentMethod { get; set; }
    }
}
