using Microsoft.AspNetCore.Identity;
using CosmeticShop.Domain.Services;
using CosmeticShop.Domain.Entities;

namespace IdentityPasswordHasher
{
    /// <summary>
    /// Provides password hashing and verification for the Customer entity.
    /// Implements the IAppPasswordHasher interface for customer-specific password operations.
    /// </summary>
    public class CustomerPasswordHasher : IAppPasswordHasher<Customer>
    {
        private readonly PasswordHasher<Customer> _passwordHasher = new();

        /// <summary>
        /// Hashes the provided password for a specific customer entity.
        /// </summary>
        /// <param name="customer">The customer entity for which the password is being hashed. Must not be null.</param>
        /// <param name="password">The plain text password to hash. Must not be null.</param>
        /// <returns>A hashed representation of the password.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the customer or password argument is null.</exception>
        public string HashPassword(Customer customer, string password)
        {
            ArgumentNullException.ThrowIfNull(nameof(customer));
            ArgumentNullException.ThrowIfNull(nameof(password));
            return _passwordHasher.HashPassword(customer, password);
        }

        /// <summary>
        /// Verifies whether the provided password matches the hashed password for a customer.
        /// Also indicates if the password needs to be rehashed.
        /// </summary>
        /// <param name="customer">The customer entity whose password is being verified. Must not be null.</param>
        /// <param name="hashPassword">The stored hashed password to compare. Must not be null.</param>
        /// <param name="providePassword">The plain text password provided by the customer for verification. Must not be null.</param>
        /// <param name="rehashNeeded">Outputs whether the password should be rehashed (due to an outdated hash algorithm).</param>
        /// <returns>True if the provided password matches the hashed password; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any of the customer, hashPassword, or providePassword arguments are null.</exception>
        public bool VerifyHashedPassword(Customer customer, string hashPassword, string providePassword, out bool rehashNeeded)
        {
            ArgumentNullException.ThrowIfNull(nameof(customer));
            ArgumentNullException.ThrowIfNull(nameof(hashPassword));
            ArgumentNullException.ThrowIfNull(nameof(providePassword));

            var result = _passwordHasher.VerifyHashedPassword(customer, hashPassword, providePassword);
            rehashNeeded = result == PasswordVerificationResult.SuccessRehashNeeded;
            return result != PasswordVerificationResult.Failed;
        }
    }
}
