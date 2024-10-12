using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Enumerations
{
    /// <summary>
    /// Represents the available shipping methods for an order.
    /// </summary>
    public enum ShippingMethod
    {
        /// <summary>
        /// Shipping via mail.
        /// </summary>
        Mail,

        /// <summary>
        /// Shipping via courier.
        /// </summary>
        Courier,

        /// <summary>
        /// Pickup from the store.
        /// </summary>
        InStorePickup
    }
}
