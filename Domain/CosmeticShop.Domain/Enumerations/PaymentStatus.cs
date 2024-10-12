using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Enumerations
{
    /// <summary>
    /// Represents the different statuses a payment can have.
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// Indicates that the payment has been initiated but not yet processed.
        /// </summary>
        Pending,

        /// <summary>
        /// Indicates that the payment has been successfully processed.
        /// </summary>
        Completed,

        /// <summary>
        /// Indicates that the payment was failed.
        /// </summary>
        Failed,

        /// <summary>
        /// Indicates that the payment was refunded.
        /// </summary>
        Refunded,

        /// <summary>
        /// Indicates that the payment has been canceled.
        /// </summary>
        Canceled
    }
}
