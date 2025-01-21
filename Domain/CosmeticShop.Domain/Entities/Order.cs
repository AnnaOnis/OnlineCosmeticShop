using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticShop.Domain.Enumerations;

namespace CosmeticShop.Domain.Entities
{
    /// <summary>
    /// Represents a customer's order.
    /// </summary>
    public class Order : IEntity
    {
        /// <summary>
        /// Gets the unique identifier for the order.
        /// </summary>
        [Key]
        public Guid Id { get; init; }

        /// <summary>
        /// Gets the unique identifier of the customer who placed the order.
        /// </summary>
        [Required]
        public Guid CustomerId { get; init; }

        /// <summary>
        /// Gets or sets the date when the order was placed.
        /// </summary>
        [Required]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the status of the order.
        /// </summary>
        [Required]
        public OrderStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the total quantity of items in the order.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Total quantity must be at least 1.")]
        public int TotalQuantity { get; set; }

        /// <summary>
        /// Gets or sets the total amount for the order.
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the shipping method for the order.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public ShippingMethod OrderShippingMethod{ get; set; }

        /// <summary>
        /// Gets or sets the payment method used for the order.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public PaymentMethod OrderPaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the customer associated with this order.
        /// </summary>
        public Customer Customer { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of items included in the order.
        /// </summary>
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        /// <summary>
        /// Parameterless constructor for Entity Framework.
        /// </summary>
        protected Order() 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class based on the provided cart,
        /// shipping method, and payment method.
        /// </summary>
        /// <param name="cart">The cart from which to create the order. Contains the items to be ordered.</param>
        /// <param name="shippingMethod">The shipping method for the order. Must be a valid value of <see cref="ShippingMethod"/>.</param>
        /// <param name="paymentMethod">The payment method for the order. Must be a valid value of <see cref="PaymentMethod"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided shipping or payment method is invalid.</exception>
        public Order(Guid customerId,
                     int totalQuatity,
                     decimal totalEmount,
                     ShippingMethod shippingMethod,
                     PaymentMethod paymentMethod)
        {
            if (!Enum.IsDefined(typeof(ShippingMethod), shippingMethod))
                throw new ArgumentOutOfRangeException(nameof(shippingMethod), "Invalid shipping method.");

            if (!Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
                throw new ArgumentOutOfRangeException(nameof(paymentMethod), "Invalid payment method.");


            Id = Guid.NewGuid();
            CustomerId = customerId;
            OrderDate = DateTime.UtcNow;
            Status = OrderStatus.Pending;
            TotalQuantity = totalQuatity;
            TotalAmount = totalEmount;
            OrderShippingMethod = shippingMethod;
            OrderPaymentMethod = paymentMethod;
            OrderItems = new List<OrderItem>();
        }

    }
}
