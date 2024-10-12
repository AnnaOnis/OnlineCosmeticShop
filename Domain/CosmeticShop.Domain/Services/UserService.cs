using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Enumerations;
using CosmeticShop.Domain.Exceptions.Customer;
using CosmeticShop.Domain.Exceptions.Users;
using Microsoft.Extensions.Logging;

namespace CosmeticShop.Domain.Services
{

    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppPasswordHasher<User> _hasher;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, Services.IAppPasswordHasher<User> hasher, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Adds a new user with a specified role.
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <param name="firstName">User's first name</param>
        /// <param name="lastName">User's last name</param>
        /// <param name="role">The role to assign to the user (Admin, Moderator, etc.)</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="firstName"/>, <paramref name="lastName"/>, 
        /// <paramref name="email"/>, <paramref name="password"/>, <paramref name="role"/> is <c>null</c>.</exception>
        /// <exception cref="EmailAlreadyExistsException">Thrown when the user with given email has already been registered.</exception>
        public async Task AddUser(string email, 
                            string password, 
                            string firstName, 
                            string lastName, 
                            RoleType role, 
                            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(firstName, nameof(firstName));
            ArgumentNullException.ThrowIfNull(lastName, nameof(lastName));
            ArgumentNullException.ThrowIfNull(email, nameof(email));
            ArgumentNullException.ThrowIfNull(password, nameof(password));
            ArgumentNullException.ThrowIfNull(role, nameof(role));

            var existingUser = _unitOfWork.UserRepository.FindByEmail(email, cancellationToken);
            if (existingUser != null)
                throw new EmailAlreadyExistsException(message: "The user with this email has already been registered.");


            var user = new User(firstName, lastName, email, password, role);
            var hashedPassword = _hasher.HashPassword(user, password);
            user.PasswordHash = hashedPassword;

            await _unitOfWork.UserRepository.Add(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Logs in a user (Admin, Moderator, etc.) by verifying credentials.
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns>User object on successful login</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="email"/>, <paramref name="password"/> is <c>null</c>.</exception>
        /// <exception cref="UserNotFoundException">Thrown when user with given email not found.</exception>
        /// <exception cref="InvalidPasswordException">Thrown when given password is invalid.</exception>
        public async Task<User> Login(string email, string password, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(email, nameof(email));
            ArgumentNullException.ThrowIfNull(password, nameof(password));

            var user = await _unitOfWork.UserRepository.FindByEmail(email, cancellationToken);
            if (user == null)
            {
                throw new UserNotFoundException("User with given email not found.");
            }

            var isPasswordValid = _hasher.VerifyHashedPassword(user, user.PasswordHash, password, out var rehashNeeded);
            if (!isPasswordValid)
            {
                throw new InvalidPasswordException("Invalid password");
            }

            if (rehashNeeded)
            {
                user.PasswordHash = _hasher.HashPassword(user, password);
                await _unitOfWork.UserRepository.Update(user, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return user;
        }

        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <returns>List of User objects</returns>
        public async Task<IReadOnlyList<User>> GetAllUsers(CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.GetAll(cancellationToken);
        }

        /// <summary>
        /// Retrieves information about a specific user by their ID.
        /// </summary>
        /// <param name="userId">User's ID</param>
        /// <returns>User object</returns>
        public async Task<User> GetUserInfo(Guid userId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.GetById(userId, cancellationToken);
        }

        /// <summary>
        /// Updates the profile of a user.
        /// </summary>
        /// <param name="userId">User's ID</param>
        /// <param name="newEmail">New email address</param>
        /// <param name="newFirstName">New first name</param>
        /// <param name="newLastName">New last name</param>
        /// <exception cref="UserNotFoundException">Thrown when user not found.</exception>
        public async Task UpdateUser(Guid userId,
                                     string newEmail,
                                     string newFirstName,
                                     string newLastName,
                                     CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId, cancellationToken);
            if (user == null)
                throw new UserNotFoundException("User not found.");

            user.Email = newEmail;
            user.FirstName = newFirstName;
            user.LastName = newLastName;

            await _unitOfWork.UserRepository.Update(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Resets the user's password.
        /// </summary>
        /// <param name="userId">User's ID</param>
        /// <param name="newPassword">New password</param>
        /// <exception cref="UserNotFoundException">Thrown when user is not found.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="newPassword"/> is null</exception>
        public async Task ResetPassword(Guid userId, string newPassword, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(newPassword, nameof(newPassword));

            var user = await _unitOfWork.UserRepository.GetById(userId, cancellationToken);
            if (user == null)
                throw new UserNotFoundException("User not found.");

            user.PasswordHash = _hasher.HashPassword(user, newPassword);

            await _unitOfWork.UserRepository.Update(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="userId">User's ID</param>
        /// <exception cref="UserNotFoundException">Thrown when user not found.</exception>
        public async Task DeleteUser(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId, cancellationToken);
            if (user == null)
                throw new UserNotFoundException("User not found.");

            await _unitOfWork.UserRepository.Delete(user.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

}
