using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Enumerations
{
    /// <summary>
    /// Represents the available payment methods for an order.
    /// </summary>
    public enum PaymentMethod
    {
        /// <summary>
        /// Payment via online card payment.
        /// </summary>
        CardOnline,

        /// <summary>
        /// Payment on delivery.
        /// </summary>
        CashOnDelivery
    }
}
