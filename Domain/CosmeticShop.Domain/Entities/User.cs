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
    /// Represents an application user.
    /// </summary>
    public class User : IEntity
    {
        /// <summary>
        /// Gets the unique identifier of the user.
        /// </summary>
        [Key]
        public Guid Id { get; init; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password hash of the user.
        /// </summary>
        [Required(ErrorMessage = "Password hash is required")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        [Required(ErrorMessage = "Role is required")]
        public RoleType Role { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="passwordHash">The hashed password of the user.</param>
        /// <param name="role">The role of the user.</param>
        /// <exception cref="ArgumentException">Thrown when any required argument is invalid.</exception>
        public User(string firstName, string lastName, string email, string passwordHash, RoleType role)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException("First name cannot be null or whitespace.", nameof(firstName));

            if (firstName.Length > 50)
                throw new ArgumentException("First name cannot be longer than 50 characters.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentNullException("Last name cannot be null or whitespace.", nameof(lastName));

            if (lastName.Length > 50)
                throw new ArgumentException("Last name cannot be longer than 50 characters.", nameof(lastName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Email cannot be null or whitespace.", nameof(email));

            if (!new EmailAddressAttribute().IsValid(email))
                throw new ArgumentException("Invalid email format.", nameof(email));

            if (email.Length > 100)
                throw new ArgumentException("Email cannot be longer than 100 characters.", nameof(email));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentNullException("Password hash cannot be null or whitespace.", nameof(passwordHash));

            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }
    }
}
