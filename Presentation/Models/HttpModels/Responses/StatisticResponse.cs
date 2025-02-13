using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpModels.Responses
{
    public class StatisticResponse
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "OrderCount must be at least 0.")]
        public int OrderCount { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        public decimal TotalRevenue { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "NewCustomerCount must be at least 0.")]
        public int NewCustomerCount { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "approvedReviewCount must be at least 0.")]
        public int ApprovedReviewCount { get; set; }

    }
}
