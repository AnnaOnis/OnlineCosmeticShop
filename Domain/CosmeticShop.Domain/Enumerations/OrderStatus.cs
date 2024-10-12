using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Enumerations
{
    /// <summary>
    /// Represents the possible statuses for an order.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// The order is placed but not yet processed.
        /// </summary>
        Pending,

        /// <summary>
        /// The order is being processed.
        /// </summary>
        Processing,

        /// <summary>
        /// The order has been shipped.
        /// </summary>
        Shipped,

        /// <summary>
        /// The order has been delivered.
        /// </summary>
        Delivered,

        /// <summary>
        /// The order has been canceled.
        /// </summary>
        Canceled,

        /// <summary>
        /// The order has been returned.
        /// </summary>
        Returned
    }
}
