using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpModels.Requests
{
    public class FavoritesRequestDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
    }
}
