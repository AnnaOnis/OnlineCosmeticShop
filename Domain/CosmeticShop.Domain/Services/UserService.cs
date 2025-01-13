using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Enumerations;
using CosmeticShop.Domain.Exceptions.Customer;
using CosmeticShop.Domain.Exceptions.Users;
using CosmeticShop.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

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
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="role"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="firstName"/>, <paramref name="lastName"/>, 
        /// <paramref name="email"/>, <paramref name="password"/> is <c>null</c> or contains only a space.</exception>
        /// <exception cref="EmailAlreadyExistsException">Thrown when the user with given email has already been registered.</exception>
        public async Task AddUser(string email, 
                            string password, 
                            string firstName, 
                            string lastName, 
                            RoleType role, 
                            CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));
            ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
            ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));
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
        /// <exception cref="ArgumentException">Thrown when <paramref name="email"/>, <paramref name="password"/> is <c>null</c> or contains only a space.</exception>
        /// <exception cref="UserNotFoundException">Thrown when user with given email not found.</exception>
        /// <exception cref="InvalidPasswordException">Thrown when given password is invalid.</exception>
        public async Task<User> Login(string email, string password, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
            ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

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
        /// <param name="filter">Optional filter by user FirstName, LastName, Email or Role.</param>
        /// <param name="sortField">Field to sort by (e.g., "FirstName", "LastName", "Email").</param>
        /// <param name="sortOrder">Specifies the sort order.</param>
        /// <param name="pageNumber">Specifies the page number</param>
        /// <param name="pageSize">Specifies the page size</param>
        /// <returns>List of User objects</returns>
        public async Task<(IReadOnlyList<User>, int)> GetAllSortedUsers(CancellationToken cancellationToken,
                                                                   string? filter = null,
                                                                   string? sortField = "LastName",
                                                                   string sortOrder = "asc",
                                                                   int pageNumber = 1,
                                                                   int pageSize = 10)
        {
            // Метод фильтрации
            Expression<Func<User, bool>>? filterExpression = null;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                filterExpression = u =>
                    u.FirstName.Contains(filter) ||
                    u.LastName.Contains(filter) ||
                    u.Email.Contains(filter) ||
                    u.Role.Equals(filter);
            }

            // Метод сортировки
            Func<IQueryable<User>, IOrderedQueryable<User>>? sortExpression = null;
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                sortOrder = sortOrder == "asc" || sortOrder == "desc" ? sortOrder : "asc";
                sortExpression = sortField switch
                {
                    "FirstName" => sortOrder == "asc"
                        ? q => q.OrderBy(o => o.FirstName)
                        : q => q.OrderByDescending(o => o.FirstName),
                    "Email" => sortOrder == "asc"
                        ? q => q.OrderBy(o => o.Email)
                        : q => q.OrderByDescending(o => o.Email),
                    _ => sortOrder == "asc"
                        ? q => q.OrderBy(o => o.LastName)
                        : q => q.OrderByDescending(o => o.LastName),
                };
            }

            // Вызов универсального метода GetAllSorted
            return await _unitOfWork.UserRepository.GetAllSorted(
                cancellationToken,
                filter: filterExpression,  // Фильтр
                sorter: sortExpression,   // Сортировка
                pageNumber: pageNumber,    // Пагинация - номер страницы
                pageSize: pageSize         // Пагинация - размер страницы
            );
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
        /// <exception cref="ArgumentException">Thrown when <paramref name="newEmail"/>, <paramref name="newFirstName"/>, 
        /// <paramref name="newLastName"/> is <c>null</c> or contains only a space.</exception>
        /// <exception cref="UserNotFoundException">Thrown when user not found.</exception>
        public async Task<User> UpdateUser(Guid userId,
                                     string newEmail,
                                     string newFirstName,
                                     string newLastName,
                                     CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newFirstName, nameof(newFirstName));
            ArgumentException.ThrowIfNullOrWhiteSpace(newLastName, nameof(newLastName));
            ArgumentException.ThrowIfNullOrWhiteSpace(newEmail, nameof(newEmail));

            var user = await _unitOfWork.UserRepository.GetById(userId, cancellationToken);
            if (user == null)
                throw new UserNotFoundException("User not found.");

            user.Email = newEmail;
            user.FirstName = newFirstName;
            user.LastName = newLastName;

            await _unitOfWork.UserRepository.Update(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user;
        }

        /// <summary>
        /// Resets the user's password.
        /// </summary>
        /// <param name="userId">User's ID</param>
        /// <param name="newPassword">New password</param>
        /// <exception cref="UserNotFoundException">Thrown when user is not found.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="newPassword"/> is null or contains only a space</exception>
        public async Task ResetPassword(Guid userId, string newPassword, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newPassword, nameof(newPassword));

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

        public async Task<bool> ExistsByEmail(string email, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));

            var existingUser = await _unitOfWork.UserRepository.FindByEmail(email, cancellationToken);
            if (existingUser == null) return false;
            return true;
        }
    }

}
