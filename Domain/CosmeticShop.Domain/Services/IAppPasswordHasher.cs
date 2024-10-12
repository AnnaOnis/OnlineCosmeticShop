using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Services
{
    /// <summary>
    /// Provides methods for hashing and verifying passwords for a specific user type.
    /// </summary>
    /// <typeparam name="TUser">The type of the user for which passwords are being hashed and verified. Typically a class representing a user entity.</typeparam>
    public interface IAppPasswordHasher<TUser> where TUser : class
    {
        /// <summary>
        /// Generates a hashed representation of a given password for a specific user.
        /// </summary>
        /// <param name="user">The user for whom the password is being hashed.</param>
        /// <param name="password">The plain-text password to hash.</param>
        /// <returns>A hashed password string.</returns>
        string HashPassword(TUser user, string password);

        /// <summary>
        /// Verifies the hashed password against the provided plain-text password for a specific user.
        /// </summary>
        /// <param name="user">The user for whom the password is being verified.</param>
        /// <param name="hashPassword">The previously hashed password.</param>
        /// <param name="providePassword">The plain-text password to verify.</param>
        /// <param name="rehashNeeded">Indicates whether the password hash needs to be updated (rehashing is needed if an older or less secure algorithm was used).</param>
        /// <returns><c>true</c> if the provided password matches the hashed password; otherwise, <c>false</c>.</returns>
        bool VerifyHashedPassword(TUser user, string hashPassword, string providePassword, out bool rehashNeeded);
    }
}
