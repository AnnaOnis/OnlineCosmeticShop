using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Enumerations;
using CosmeticShop.Domain.Exceptions;
using CosmeticShop.Domain.Exceptions.Order;
using CosmeticShop.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Services
{
    /// <summary>
    /// Service responsible for managing orders in the cosmetics store.
    /// Handles order creation, retrieval, and updating order statuses.
    /// </summary>
    public class OrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CartService> _logger;

        public OrderService(IUnitOfWork unitOfWork, ILogger<CartService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates an order for the specified customer based on the contents of their cart.
        /// </summary>
        /// <param name="customerId">The ID of the customer placing the order.</param>
        /// <param name="shippingMethod">The selected shipping method for the order.</param>
        /// <param name="paymentMethod">The selected payment method for the order.</param>
        /// <returns>The created Order object.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the cart is empty or not found.</exception>
        public async Task<Order> CreateOrderAsync(Guid customerId, 
                                                  ShippingMethod shippingMethod, 
                                                  PaymentMethod paymentMethod,
                                                  CancellationToken cancellationToken)
        {
            var cart = await _unitOfWork.CartRepository.GetCartByCustomerId(customerId, cancellationToken);
            if (cart == null || !cart.CartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty or not found.");
            }

            var order = new Order(cart, shippingMethod, paymentMethod);
            await _unitOfWork.OrderRepository.Add(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return order;
        }

        /// <summary>
        /// Retrieves all orders for the specified customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer whose orders are to be retrieved.</param>
        /// <returns>A list of orders for the specified customer.</returns>
        public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(Guid customerId, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersByCustomerId(customerId, cancellationToken);
            orders = orders.OrderBy(o => o.OrderDate);
            return orders;
        }

        /// <summary>
        /// Retrieves the details of a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <returns>The Order object containing the order details.</returns>
        /// <exception cref="OrderNotFoundException">Thrown when the order not found.</exception>
        public async Task<Order> GetOrderDetailsAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.OrderRepository.GetById(orderId, cancellationToken);
            if (order == null)
            {
                throw new OrderNotFoundException("Order not found.");
            }

            return order;
        }

        /// <summary>
        /// Updates the status of an order. This operation is restricted to administrators.
        /// </summary>
        /// <param name="orderId">The ID of the order to update.</param>
        /// <param name="newStatus">The new status of the order.</param>
        /// <exception cref="OrderNotFoundException">Thrown when the order not found.</exception>
        public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.OrderRepository.GetById(orderId, cancellationToken);
            if (order == null)
            {
                throw new OrderNotFoundException("Order not found.");
            }

            order.Status = newStatus;
            await _unitOfWork.OrderRepository.Update(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves all orders across all users, with options for filtering, sorting, and searching.
        /// This operation is restricted to administrators.
        /// </summary>
        /// <param name="searchTerm">Optional search term to filter orders by customer name, product, etc.</param>
        /// <param name="statusFilter">Optional filter by order status (e.g., Pending, Shipped).</param>
        /// <param name="sortBy">Optional sorting by fields like OrderDate or TotalAmount.</param>
        /// <param name="ascending">Indicates whether the sort should be ascending.</param>
        /// <returns>A list of filtered and sorted orders.</returns>
        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync(CancellationToken cancellationToken,
                                                                string? searchTerm = null, 
                                                                OrderStatus? statusFilter = null, 
                                                                string sortBy = "OrderDate", 
                                                                bool ascending = true)
        {
            var orders = await _unitOfWork.OrderRepository.GetAll(cancellationToken);

            // Apply search term if provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                orders = orders.Where(o => o.Customer.FirstName.Contains(searchTerm) ||
                                      o.Customer.LastName.Contains(searchTerm) ||
                                      o.OrderItems.Any(i => i.Product.Name.Contains(searchTerm)))
                                        .ToList().AsReadOnly();
            }

            // Apply status filter if provided
            if (statusFilter.HasValue)
            {
                orders = orders.Where(o => o.Status == statusFilter.Value).ToList().AsReadOnly();
            }

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                // Apply sorting
                orders = SortOrders(sortBy, ascending, orders).ToList().AsReadOnly();
            }

            return orders;
        }

        /// <summary>
        /// Sorts the collection of orders based on the provided field and order.
        /// </summary>
        /// <param name="orders">The collection of orders to sort.</param>
        /// <param name="sortBy">The field to sort by.</param>
        /// <param name="ascending">Indicates whether the sort should be ascending.</param>
        /// <returns>The sorted collection of orders.</returns>
        private IEnumerable<Order> SortOrders(string sortBy, bool ascending, IEnumerable<Order> orders)
        {
            orders = sortBy switch
            {
                "TotalAmount" => ascending ? orders.OrderBy(o => o.TotalAmount) : orders.OrderByDescending(o => o.TotalAmount),
                "TotalQuantity" => ascending ? orders.OrderBy(o => o.TotalQuantity) : orders.OrderByDescending(o => o.TotalQuantity),
                _ => ascending ? orders.OrderBy(o => o.OrderDate) : orders.OrderByDescending(o => o.OrderDate),
            };
            return orders;
        }
    }
    
}
