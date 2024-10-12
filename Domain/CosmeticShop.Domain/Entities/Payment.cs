using CosmeticShop.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Entities
{
    /// <summary>
    /// Represents a payment for an order.
    /// </summary>
    public class Payment : IEntity
    {
        /// <summary>
        /// Gets the unique identifier for the payment.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Gets the unique identifier for the associated order.
        /// </summary>
        [Required(ErrorMessage = "OrderId is required.")]
        public Guid OrderId { get; init; }

        /// <summary>
        /// Gets the unique identifier for the customer making the payment.
        /// </summary>
        [Required(ErrorMessage = "CustomerId is required.")]
        public Guid CustomerId { get; init; }

        /// <summary>
        /// Gets or sets the date when the payment was made.
        /// </summary>
        [Required(ErrorMessage = "PaymentDate is required.")]
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the status of the payment.
        /// </summary>
        [Required(ErrorMessage = "Status is required.")]
        public PaymentStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the method used for the payment.
        /// </summary>
        [Required(ErrorMessage = "Method is required.")]
        public PaymentMethod Method { get; set; }

        // Navigation properties
        public Order Order { get; set; } = null!;
        public Customer Customer { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="Payment"/> class.
        /// </summary>
        /// <param name="orderId">The unique identifier for the associated order.</param>
        /// <param name="customerId">The unique identifier for the customer making the payment.</param>
        /// <param name="paymentDate">The date when the payment was made.</param>
        /// <param name="status">The status of the payment.</param>
        /// <param name="method">The method used for the payment.</param>
        /// <exception cref="ArgumentNullException">Thrown when any required argument is null.</exception>
        /// <exception cref="ArgumentException">Thrown when any Guid argument is empty.</exception>
        public Payment(Guid orderId, Guid customerId, PaymentMethod method)
        {
            if (!Enum.IsDefined(typeof(PaymentMethod), method))
                throw new ArgumentOutOfRangeException(nameof(method), "Invalid payment method.");

            Id = Guid.NewGuid();
            OrderId = orderId;
            CustomerId = customerId;
            PaymentDate = DateTime.Now;
            Status = PaymentStatus.Pending;
            Method = method;
        }
    }
}
