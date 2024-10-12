using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Exceptions.Customer;
using CosmeticShop.Domain.Exceptions.Users;
using CosmeticShop.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
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
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="firstName"/>, <paramref name="lastName"/>, <paramref name="email"/>, 
        /// <paramref name="password"/>, <paramref name="phoneNumber"/> or <paramref name="shippingAddress"/> is <c>null</c>.</exception>
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
            ArgumentNullException.ThrowIfNull(firstName, nameof(firstName));
            ArgumentNullException.ThrowIfNull(lastName, nameof(lastName));
            ArgumentNullException.ThrowIfNull(email, nameof(email));
            ArgumentNullException.ThrowIfNull(password, nameof(password));
            ArgumentNullException.ThrowIfNull(nameof(phoneNumber));
            ArgumentNullException.ThrowIfNull(nameof(shippingAddress));

            var existingCustomer = _unitOfWork.CustomerRepository.FindByEmail(email, cancellationToken);
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
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="email"/> or <paramref name="password"/> is <c>null</c>.</exception>
        /// <exception cref="CustomerNotFoundException">Thrown when customer with given email not found.</exception>
        /// <exception cref="InvalidPasswordException">Thrown when given password is invalid.</exception>
        public async Task<Customer> Login(string email, string password, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(email, nameof(email));
            ArgumentNullException.ThrowIfNull(password, nameof(password));

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

        /// <summary>
        /// Retrieves a list of all customers.
        /// </summary>
        /// <returns>List of Customer objects</returns>
        public async Task<IReadOnlyList<Customer>> GetAllCustomers(CancellationToken cancellationToken)
        {
            return await _unitOfWork.CustomerRepository.GetAll(cancellationToken);
        }

        /// <summary>
        /// Retrieves information about the current customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer</param>
        /// <returns>Customer object</returns>
        public async Task<Customer> GetCustomerInfo(Guid customerId, CancellationToken cancellationToken)
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
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="newFirstName"/>, <paramref name="newlastName"/>, <paramref name="newEmail"/>, 
        /// <paramref name="newPhoneNumber"/> or <paramref name="newShippingAddress"/> is <c>null</c>.</exception>
        /// <exception cref="CustomerNotFoundException">Thrown when customer is not found.</exception>
        public async Task<Customer> UpdateCustomerProfile(Guid customerId, 
                                                string newEmail, 
                                                string newFirstName, 
                                                string newlastName, 
                                                string newPhoneNumber,
                                                string newShippingAddress,
                                                CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(newFirstName, nameof(newFirstName));
            ArgumentNullException.ThrowIfNull(newlastName, nameof(newlastName));
            ArgumentNullException.ThrowIfNull(newEmail, nameof(newEmail));
            ArgumentNullException.ThrowIfNull(newPhoneNumber, nameof(newPhoneNumber));
            ArgumentNullException.ThrowIfNull(newShippingAddress, nameof(newPhoneNumber));

            var customer = await _unitOfWork.CustomerRepository.GetById(customerId, cancellationToken);
            if (customer == null)
                throw new CustomerNotFoundException("Customer not found.");

            
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
        /// <exception cref="CustomerNotFoundException">Thrown when customer is not found.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="newPassword"/> is null</exception>
        public async Task ResetPassword(Guid customerId, string newPassword, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(newPassword, nameof(newPassword));

            var customer = await _unitOfWork.CustomerRepository.GetById(customerId, cancellationToken);
            if (customer == null)
                throw new CustomerNotFoundException("Customer not found.");

            customer.PasswordHash = _hasher.HashPassword(customer, newPassword);

            await _unitOfWork.CustomerRepository.Update(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Deletes a customer by their ID.
        /// </summary>
        /// <param name="customerId">Customer's ID</param>
        /// <exception cref="CustomerNotFoundException">Thrown when customer not found.</exception>
        public async Task DeleteCustomer(Guid customerId, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.CustomerRepository.GetById(customerId, cancellationToken);
            if (customer == null)
                throw new CustomerNotFoundException("Customer not found.");

            await _unitOfWork.CustomerRepository.Delete(customer.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

}
