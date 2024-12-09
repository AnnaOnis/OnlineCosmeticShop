﻿using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Enumerations;
using CosmeticShop.Domain.Exceptions;
using CosmeticShop.Domain.Exceptions.Order;
using CosmeticShop.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
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
            if (cart == null || cart.CartItems.Count == 0)
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
            orders = orders.OrderByDescending(o => o.OrderDate);
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
        /// <param name="filter">Optional search term to filter orders by customer name, product, etc.</param>
        /// <param name="sortField">Field to sort by (OrderDate, TotalAmount, TotalQuantity).</param>
        /// <param name="sortOrder">Indicates whether the sort should be ascending.</param>
        /// <param name="pageNumber">Specifies the page number</param>
        /// <param name="pageSize">Specifies the page size</param>
        /// <returns>A list of filtered and sorted orders.</returns>
        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync(CancellationToken cancellationToken,
                                                                  string? filter = null,
                                                                  string? sortField = "OrderDate",
                                                                  string sortOrder = "asc",
                                                                  int pageNumber = 1,
                                                                  int pageSize = 10)
        {
            // Метод фильтрации
            Expression<Func<Order, bool>>? filterExpression = null;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                filterExpression = o => o.Customer.FirstName.Contains(filter) ||
                                      o.Customer.LastName.Contains(filter) ||
                                      o.OrderItems.Any(i => i.Product.Name.Contains(filter) ||
                                      o.Status.ToString().Equals(filter, StringComparison.OrdinalIgnoreCase));
            }

            // Метод сортировки
            Func<IQueryable<Order>, IOrderedQueryable<Order>>? sortExpression = null;

            if (!string.IsNullOrWhiteSpace(sortField))
            {
                sortOrder = sortOrder == "asc" || sortOrder == "desc" ? sortOrder : "asc";

                sortExpression = sortField switch
                {
                    "TotalAmount" => sortOrder == "asc" 
                        ? q => q.OrderBy(o => o.TotalAmount) 
                        : q => q.OrderByDescending(o => o.TotalAmount),
                    "TotalQuantity" => sortOrder == "asc" 
                        ? q => q.OrderBy(o => o.TotalQuantity) 
                        : q => q.OrderByDescending(o => o.TotalQuantity),
                    _ => sortOrder == "asc" 
                        ? q => q.OrderBy(o => o.OrderDate) 
                        : q => q.OrderByDescending(o => o.OrderDate),
                };
            }

            return await _unitOfWork.OrderRepository.GetAllSorted(cancellationToken,
                filter: filterExpression,
                sorter: sortExpression,
                pageNumber: pageNumber,
                pageSize: pageSize);
        }

        public async Task DeleteOrder(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.OrderRepository.GetById(orderId, cancellationToken);
            if (order == null)
            {
                throw new OrderNotFoundException("Order not found.");
            }

            await _unitOfWork.OrderRepository.Delete(orderId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }   
}
