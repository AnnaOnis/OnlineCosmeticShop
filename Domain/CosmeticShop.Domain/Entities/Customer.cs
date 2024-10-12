using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Entities
{
    /// <summary>
    /// Represents a customer of the store.
    /// </summary>
    public class Customer : IEntity
    {
        /// <summary>
        /// Gets the unique identifier of the customer.
        /// </summary>
        [Key]
        public Guid Id { get; init; }

        /// <summary>
        /// Gets or sets the first name of the customer.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the customer.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the customer.
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password hash of the customer.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the customer.
        /// </summary>
        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the shipping address of the customer.
        /// </summary>
        [MaxLength(250)]
        public string ShippingAddress { get; set; }

        /// <summary>
        /// Gets or sets the date when the customer registered.
        /// </summary>
        [Required]
        public DateTime DateRegistered { get; set; }

        // Navigation properties

        /// <summary>
        /// Gets or sets the collection of favorite products of the customer.
        /// </summary>
        public ICollection<Favorite> Favorites { get; set; }

        /// <summary>
        /// Gets or sets the collection of orders placed by the customer.
        /// </summary>
        public ICollection<Order> Orders { get; set; }

        /// <summary>
        /// Gets or sets the collection of payments made by the customer.
        /// </summary>
        public ICollection<Payment> Payments { get; set; }

        /// <summary>
        /// Gets or sets the collection of reviews made by the customer.
        /// </summary>
        public ICollection<Review> Reviews { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class with the specified details.
        /// </summary>
        /// <param name="firstName">The first name of the customer.</param>
        /// <param name="lastName">The last name of the customer.</param>
        /// <param name="email">The email address of the customer.</param>
        /// <param name="passwordHash">The hashed password of the customer.</param>
        /// <param name="phoneNumber">The phone number of the customer.</param>
        /// <param name="shippingAddress">The shipping address of the customer.</param>
        public Customer(string firstName, string lastName, string email, string passwordHash, string phoneNumber, string shippingAddress)
        {
            Id = Guid.NewGuid();
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
            ShippingAddress = shippingAddress ?? throw new ArgumentNullException(nameof(shippingAddress));
            DateRegistered = DateTime.Now;

            Favorites = new List<Favorite>();
            Orders = new List<Order>();
            Payments = new List<Payment>();
            Reviews = new List<Review>();
        }

    }
}
