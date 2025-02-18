using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticShop.Domain.Enumerations;

namespace HttpModels.Requests
{
    public class PaymentUpdateStatusRequestDto
    {
        public Guid OrderId { get; set; }
        [Required]
        public PaymentStatus NewPaymentStatus { get; set; }
    }
}
