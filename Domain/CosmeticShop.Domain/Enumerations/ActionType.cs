using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Enumerations
{
    /// <summary>
    /// Represents the various actions a user can perform in the system.
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// Action for adding a new product.
        /// </summary>
        AddProduct,        // Adding a product

        /// <summary>
        /// Action for updating an existing product's information.
        /// </summary>
        UpdateProduct,     // Updating product information

        /// <summary>
        /// Action for deleting a product from the catalog.
        /// </summary>
        DeleteProduct,     // Deleting a product

        /// <summary>
        /// Action for creating a new order.
        /// </summary>
        CreateOrder,       // Creating an order

        /// <summary>
        /// Action for updating an existing order.
        /// </summary>
        UpdateOrder,       // Updating an order

        /// <summary>
        /// Action for deleting an order from the system.
        /// </summary>
        DeleteOrder,       // Deleting an order

        /// <summary>
        /// Action for approving a customer review.
        /// </summary>
        ApproveReview,     // Approving a review

        /// <summary>
        /// Action for rejecting a customer review.
        /// </summary>
        RejectReview,      // Rejecting a review

        /// <summary>
        /// Action for managing user accounts (e.g., changing roles).
        /// </summary>
        ManageUser,        // Managing user accounts

        /// <summary>
        /// Action for viewing reports or analytics.
        /// </summary>
        ViewReport,        // Viewing reports

        /// <summary>
        /// Action for other miscellaneous operations.
        /// </summary>
        Other              // Other actions
    }
}
