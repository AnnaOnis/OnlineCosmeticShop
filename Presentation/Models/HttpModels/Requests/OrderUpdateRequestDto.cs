using CosmeticShop.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpModels.Requests
{
    public class OrderUpdateRequestDto 
    {
        public Guid Id { get; set; }
        [Required]
        public OrderStatus NewStatus { get; set; }
    }
}
