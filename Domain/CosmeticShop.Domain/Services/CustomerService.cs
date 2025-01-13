using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Exceptions.Customer;
using CosmeticShop.Domain.Exceptions.Product;
using CosmeticShop.Domain.Exceptions.Users;
using CosmeticShop.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CosmeticShop.Domain.Services
{
    public class CustomerService 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppPasswordHasher<Customer> _hasher;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IUnitOfWork unitOfWork, Services.IAppPasswordHasher<Customer> hasher, ILogger<CustomerService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Registers a new customer.
        /// </summary>
        /// <param name="firstName">Customer's first name</param>
        /// <param name="lastName">Customer's last name</param>
        /// <param name="email">Customer's email address</param>
        /// <param name="password">Customer's password</param>
        /// <param name="phoneNumber">Customer's phoneNumber</param>
        /// <param name="shippingAddress">Customer's shippingAddress</param>
        /// <returns>Customer object after registration</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="firstName"/>, <paramref name="lastName"/>, <paramref name="email"/>, 
        /// <paramref name="password"/>, <paramref name="phoneNumber"/> or <paramref name="shippingAddress"/> is <c>null</c> or contains only space.</exception>
        /// <exception cref="EmailAlreadyExistsException">Thrown when the user with the entered email has already been registered.</exception>
        public async Task<Customer> Register(
                                 string firstName, 
                                 string lastName, 
                                 string email, 
                                 string password, 
                                 string phoneNumber, 
                                 string shippingAddress, 
                                 CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));
            ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
            ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(phoneNumber));
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(shippingAddress));

            var existingCustomer = await _unitOfWork.CustomerRepository.FindByEmail(email, cancellationToken);
            if (existingCustomer != null)
                throw new EmailAlreadyExistsException(message: "The customer with this email has already been registered.");

            
            var customer = new Customer(firstName, lastName, email, password, phoneNumber, shippingAddress);
            var hashedPassword = _hasher.HashPassword(customer, password);
            customer.PasswordHash = hashedPassword;

            Cart cart = new Cart(customer.Id);

            await _unitOfWork.CustomerRepository.Add(customer, cancellationToken);
            await _unitOfWork.CartRepository.Add(cart, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return customer;
        }

        /// <summary>
        /// Logs in a customer by verifying credentials.
        /// </summary>
        /// <param name="email">Customer's email</param>
        /// <param name="password">Customer's password</param>
        /// <returns>Customer object on successful login</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="email"/> or <paramref name="password"/> is <c>null</c> or contains only space.</exception>
        /// <exception cref="CustomerNotFoundException">Thrown when customer with given email not found.</exception>
        /// <exception cref="InvalidPasswordException">Thrown when given password is invalid.</exception>
        public async Task<Customer> Login(string email, string password, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
            ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

            var customer = await _unitOfWork.CustomerRepository.FindByEmail(email, cancellationToken);
            if (customer == null)
                throw new CustomerNotFoundException("Customer with given email not found.");

            var isPasswordValid = _hasher.VerifyHashedPassword(customer, customer.PasswordHash, password, out var rehashNeeded);
            if (!isPasswordValid)
            {
                throw new InvalidPasswordException("Invalid password");
            }

            if (rehashNeeded)
            {
                customer.PasswordHash = _hasher.HashPassword(customer, password);
                await _unitOfWork.CustomerRepository.Update(customer, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return customer;
        }

        public async Task<bool> ExistsByEmail(string email, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));

            var existingCustomer = await _unitOfWork.CustomerRepository.FindByEmail(email, cancellationToken);
            if (existingCustomer == null) return false;
            return true;
        }

        /// <summary>
        /// Retrieves a list of all customers.
        /// </summary>
        /// <param name="filter">Optional filter by customer FirstName, LastName, Email, PhoneNumber, DateRegistered or ShippingAddress.</param>
        /// <param name="sortField">Field to sort by (e.g., "FirstName", "LastName", "Email", "DateRegistered").</param>
        /// <param name="sortOrder">Indicates whether the sort order is ascending.</param>
        /// <param name="pageNumber">Specifies the page number</param>
        /// <param name="pageSize">Specifies the page size</param>
        /// <returns>List of Customer objects</returns>
        public async Task<(IReadOnlyList<Customer>, int)> GetAllCustomers(CancellationToken cancellationToken,
                                                                   string? filter = null,
                                                                   string? sortField = "LastName",
                                                                   string sortOrder = "asc",
                                                                   int pageNumber = 1,
                                                                   int pageSize = 10)
        {
            // Метод фильтрации
            Expression<Func<Customer, bool>>? filterExpression = null;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                DateTime dateFilter;
                bool isDateFilter = DateTime.TryParse(filter, out dateFilter);

                filterExpression = c =>
                                c.FirstName.Contains(filter) ||
                                c.LastName.Contains(filter) ||
                                c.Email.Contains(filter) ||
                                c.PhoneNumber.Contains(filter) ||
                                c.ShippingAddress.Contains(filter) ||
                                (isDateFilter && c.DateRegistered.Date == dateFilter);
            }

            // Метод сортировки
            Func<IQueryable<Customer>, IOrderedQueryable<Customer>>? sortExpression = null;
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
                    "DateRegistered" => sortOrder == "asc" 
                        ? q => q.OrderBy(o => o.DateRegistered) 
                        : q => q.OrderByDescending(o => o.DateRegistered),
                    _ => sortOrder == "asc" 
                        ? q => q.OrderBy(o => o.LastName) 
                        : q => q.OrderByDescending(o => o.LastName),
                };
            }

            var (customers, totalCustomers) =  await _unitOfWork.CustomerRepository.GetAllSorted(cancellationToken,
                filter: filterExpression,
                sorter: sortExpression,
                pageNumber: pageNumber,
                pageSize: pageSize);

            return (customers, totalCustomers);
        }

        /// <summary>
        /// Retrieves information about the current customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer</param>
        /// <returns>Customer object</returns>
        public async Task<Customer> GetCustomerById(Guid customerId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.CustomerRepository.GetById(customerId, cancellationToken);
        }

        /// <summary>
        /// Updates the customer's profile.
        /// </summary>
        /// <param name="customerId">Customer's ID</param>
        /// <param name="newEmail">New email address</param>
        /// <param name="newFirstName">New first name</param>
        /// <param name="newlastName">New last name</param>
        /// <param name="newPhoneNumber">New phone number</param>
        /// <param name="newShippingAddress">New shipping address</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="newFirstName"/>, <paramref name="newlastName"/>, <paramref name="newEmail"/>, 
        /// <paramref name="newPhoneNumber"/> or <paramref name="newShippingAddress"/> is <c>null</c> or contains only space.</exception>
        public async Task<Customer> UpdateCustomerProfile(Guid customerId, 
                                                string newEmail, 
                                                string newFirstName, 
                                                string newlastName, 
                                                string newPhoneNumber,
                                                string newShippingAddress,
                                                CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newFirstName, nameof(newFirstName));
            ArgumentException.ThrowIfNullOrWhiteSpace(newlastName, nameof(newlastName));
            ArgumentException.ThrowIfNullOrWhiteSpace(newEmail, nameof(newEmail));
            ArgumentException.ThrowIfNullOrWhiteSpace(newPhoneNumber, nameof(newPhoneNumber));
            ArgumentException.ThrowIfNullOrWhiteSpace(newShippingAddress, nameof(newPhoneNumber));

            var customer = await GetCustomerOrThrowAsync(customerId, cancellationToken);
           
            customer.FirstName = newFirstName;
            customer.LastName = newlastName;
            customer.Email = newEmail;
            customer.PhoneNumber = newPhoneNumber;
            customer.ShippingAddress = newShippingAddress;

            await _unitOfWork.CustomerRepository.Update(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return customer;
        }

        /// <summary>
        /// Resets the customer's password.
        /// </summary>
        /// <param name="customerId">Customer's ID</param>
        /// <param name="newPassword">New password</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="newPassword"/> is null or contains only space</exception>
        public async Task ResetPassword(Guid customerId, string newPassword, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newPassword, nameof(newPassword));

            var customer = await GetCustomerOrThrowAsync(customerId, cancellationToken);

            customer.PasswordHash = _hasher.HashPassword(customer, newPassword);

            await _unitOfWork.CustomerRepository.Update(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Deletes a customer by their ID.
        /// </summary>
        /// <param name="customerId">Customer's ID</param>
        public async Task DeleteCustomer(Guid customerId, CancellationToken cancellationToken)
        {
            var customer = await GetCustomerOrThrowAsync(customerId, cancellationToken);

            await _unitOfWork.CustomerRepository.Delete(customer.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a read-only list of products that are marked as favorites by a specified customer.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer whose favorite products are being retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a read-only list of favorite products.
        /// </returns>
        public async Task<IReadOnlyList<Product>> GetAllFavorites(Guid customerId, CancellationToken cancellationToken)
        {
            var customer = await GetCustomerOrThrowAsync(customerId, cancellationToken);

            var products = customer.Favorites
                .OrderByDescending(f => f.DateAdded)
                .Select(f => f.Product)
                .ToList();

            return products;
        }

        /// <summary>
        /// Adds a product to the favorites list.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ProductAlreadyInFavoritesException">Thrown if the product is already in the favorites list.</exception> 
        public async Task AddFavorite(Guid customerId, Guid productId, CancellationToken cancellationToken)
        {
            var customer = await GetCustomerOrThrowAsync(customerId, cancellationToken);

            if (customer.Favorites.Any(f => f.ProductId == productId))
            {
                throw new ProductAlreadyInFavoritesException("Product is already in favorites");
            }

            var favorite = new Favorite (customerId, productId);
            customer.Favorites.Add(favorite);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }


        /// <summary>
        /// Removes a product from the favorites list.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="CustomerNotFoundException">Thrown when customer not found.</exception>
        public async Task RemoveFavorite(Guid customerId, Guid productId, CancellationToken cancellationToken)
        {
            Customer customer = await GetCustomerOrThrowAsync(customerId, cancellationToken);

            var favorite = customer.Favorites.SingleOrDefault(f => f.ProductId == productId);
            if (favorite is not null)
            {
                customer.Favorites.Remove(favorite);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier. 
        /// Throws an exception if the customer is not found.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the customer object if found.
        /// </returns>
        /// <exception cref="CustomerNotFoundException">
        /// Thrown if the customer with the specified <paramref name="customerId"/> cannot be found.
        /// </exception>
        private async Task<Customer> GetCustomerOrThrowAsync(Guid customerId, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.CustomerRepository.GetById(customerId, cancellationToken);
            if (customer == null)
                throw new CustomerNotFoundException("Customer not found.");
            return customer;
        }


    }

}
