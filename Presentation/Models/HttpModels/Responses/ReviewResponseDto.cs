using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpModels.Responses
{
    public class ReviewResponseDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public string ReviewText { get; set; } = string.Empty;

        public int Rating { get; set; }

        public DateTime ReviewDate { get; set; }

        public bool IsApproved { get; set; }
    }
}
