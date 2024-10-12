using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityPasswordHasher
{
    /// <summary>
    /// Provides password hashing and verification functionality for <see cref="User"/> entities.
    /// Implements the <see cref="IAppPasswordHasher{TUser}"/> interface.
    /// </summary>
    public class UserPasswordHasher : IAppPasswordHasher<User>
    {
        private readonly PasswordHasher<User> _passwordHasher = new();

        /// <summary>
        /// Hashes the provided password for the given user.
        /// </summary>
        /// <param name="user">The user for whom the password is being hashed.</param>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>A hashed representation of the provided password.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/> 
        /// or <paramref name="password"/> is null.</exception>
        public string HashPassword(User user, string password)
        {
            ArgumentNullException.ThrowIfNull(nameof(user));
            ArgumentNullException.ThrowIfNull(nameof(password));
            return _passwordHasher.HashPassword(user, password);
        }

        /// <summary>
        /// Verifies the provided password against the stored hashed password.
        /// </summary>
        /// <param name="user">The user for whom the password is being verified.</param>
        /// <param name="hashPassword">The hashed password stored for the user.</param>
        /// <param name="providePassword">The plain-text password provided for verification.</param>
        /// <param name="rehashNeeded">Indicates whether the password needs to be rehashed.</param>
        /// <returns>True if the password verification succeeds; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/>, <paramref name="hashPassword"/>, 
        /// or <paramref name="providePassword"/> is null.</exception>
        public bool VerifyHashedPassword(User user, string hashPassword, string providePassword, out bool rehashNeeded)
        {
            ArgumentNullException.ThrowIfNull(nameof(user));
            ArgumentNullException.ThrowIfNull(nameof(hashPassword));
            ArgumentNullException.ThrowIfNull(nameof(providePassword));

            var result = _passwordHasher.VerifyHashedPassword(user, hashPassword, providePassword);
            rehashNeeded = result == PasswordVerificationResult.SuccessRehashNeeded;
            return result != PasswordVerificationResult.Failed;
        }
    }
}
