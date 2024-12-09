using CosmeticShop.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpModels.Requests
{
    public class OrderCreateRequestDto
    {
        [Required]
        public Guid CustomerId { get; init; }

        [Required]
        public ShippingMethod OrderShippingMethod { get; set; }

        [Required]
        public PaymentMethod OrderPaymentMethod { get; set; }
    }
}
